using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Server.Authentication;
using SharedLibrary.Util;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Network.Tcp;

public sealed class TcpServer
{
    public int Port { get; set; }
    public IpFiltering IpFiltering { get; set; }

    private Dictionary<int, IConnection> connections;
    private int highIndex;

    private bool accept;
    private Socket server;

    public TcpServer()
    {
        connections = new Dictionary<int, IConnection>();
    }

    public TcpServer(int port)
    {
        Port = port;
        connections = new Dictionary<int, IConnection>();
    }

    public void SendPing()
    {
        for (var n = 1; n <= highIndex; n++)
        {
            if (connections.ContainsKey(n))
            {
                connections[n].SendPing();
            }
        }
    }

    public void InitServer()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(new IPEndPoint(IPAddress.Any, Port));
        server.Listen(10);

        accept = true;
    }

    public void AcceptClient()
    {
        if (accept)
        {
            if (server.Poll(0, SelectMode.SelectRead))
            {
                var client = server.Accept();
                var ipAddress = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();


                if (IsValidIpAddress(ipAddress))
                {
                    Add(client, ipAddress);
                    Global.WriteLog(LogType.System, $"{ipAddress} is connected", ConsoleColor.Green);
                }
                else
                {
                    client.Close();
                    Global.WriteLog(LogType.System, $"Hacking Attempt: Invalid IpAddress {ipAddress}",
                        ConsoleColor.Red);
                }
            }
        }
    }

    private void Add(Socket client, string ipAddress)
    {
        var index = 0;

        if (connections.Count < highIndex)
        {
            // Procura por um slot que não está sendo usado.
            for (var i = 1; i <= highIndex; i++)
            {
                if (!connections.ContainsKey(i))
                {
                    index = i;
                    break;
                }
            }
        }
        else
        {
            index = ++highIndex;
        }

        var connection = new Connection(index, client, ipAddress);
        connection.OnDisconnect += OnDisconnect;

        connections.Add(index, connection);

        // Gambiarra, altera o highindex;
        Authentication.HighIndex = highIndex;
    }

    public void Stop()
    {
        accept = false;
        server.Close();
    }

    public void Remove(int index)
    {
        if (connections.ContainsKey(index))
        {
            connections.Remove(index);
        }
    }

    public void ReceiveData()
    {
        for (var n = 1; n <= highIndex; n++)
        {
            if (connections.ContainsKey(n))
            {
                connections[n].ReceiveData();
            }
        }
    }

    public void CheckConnectionTimeOut()
    {
        for (var n = 1; n <= highIndex; n++)
        {
            if (connections.ContainsKey(n))
            {
                connections[n].CheckConnectionTimeOut();
            }
        }
    }

    private void OnDisconnect(int index)
    {
        Remove(index);
    }

    private bool IsValidIpAddress(string ipAddress)
    {
        const int IpAddressArraySplit = 4;

        if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrEmpty(ipAddress))
        {
            return false;
        }

        var values = ipAddress.Split('.');
        if (values.Length != IpAddressArraySplit)
        {
            return false;
        }

        return values.All(r => byte.TryParse(r, out byte parsing));
    }
}