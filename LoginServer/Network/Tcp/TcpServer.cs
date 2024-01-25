using LoginServer.Communication;
using SharedLibrary.Communication;
using SharedLibrary.Util;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace LoginServer.Network.Tcp;

public sealed class TcpServer
{
    public Dictionary<int, Connection> Connections;
    public IpFiltering IpFiltering { get; set; }
    public int Port { get; set; }

    private int highIndex { get; set; }

    private bool accept;
    private Socket server;

    public TcpServer() { }

    public TcpServer(int port)
    {
        Port = port;
    }

    public void InitServer()
    {
        Connections = new Dictionary<int, Connection>();

        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        server.Bind(new IPEndPoint(IPAddress.Any, Port));
        server.Listen(10);

        accept = true;
    }

    public void AcceptClient()
    {
        if (accept)
            if (server.Poll(0, SelectMode.SelectRead))
            {
                var client = server.Accept();
                var ipAddress = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();

                if (!IsValidIpAddress(ipAddress))
                {
                    Global.WriteLog(LogType.System, "Hacking attempt: Ip Address is not valid", ConsoleColor.Cyan);
                    client.Close();

                    return;
                }
                else
                {
                    Global.WriteLog(LogType.Player, $"Incoming connection {ipAddress}", ConsoleColor.Yellow);
                }

                if (CanApproveIpAddress(ipAddress))
                {
                    Add(client);
                }
                else
                {
                    Global.WriteLog(LogType.Player, $"Refused connection {ipAddress}", ConsoleColor.Red);
                    client.Close();
                }
            }
    }

    public void Stop()
    {
        accept = false;
        server.Close();

        Connections.Clear();
    }

    public void ProcessClients()
    {
        for (var i = 1; i <= highIndex; i++)
        {
            if (Connections.ContainsKey(i))
            {
                Connections[i].ReceiveData();
            }

            if (Connections.ContainsKey(i))
            {
                RemoveInvalidConnections(i);
            }
        }
    }

    private void OnDisconnect(int index)
    {
        Remove(index);
    }
    public void Remove(int index)
    {
        if (Connections.ContainsKey(index))
        {
            Connections.Remove(index);
        }
    }

    private void RemoveInvalidConnections(int index)
    {
        Connections[index].CountConnectionTime();

        if (Connections[index].ConnectedTime >= Constants.ConnectionTimeOut)
        {
            Connections[index].Disconnect();
        }
    }

    private bool CanApproveIpAddress(string ipAddress)
    {
        var approve = true;

        if (IpBlockList.Enabled)
        {
            if (IpBlockList.IsIpBlocked(ipAddress))
            {
                Global.WriteLog(LogType.Player, $"Warning: Attempted IP Banned[{ipAddress}]", ConsoleColor.DarkRed);
                approve = false;
            }
            else
            {
                if (IpFiltering.CanBlockIpAddress(ipAddress))
                {
                    IpBlockList.AddIpAddress(ipAddress, false);
                    Global.WriteLog(LogType.Player, $"Warning: [{ipAddress}] has been blocked", ConsoleColor.DarkBlue);
                    approve = false;
                }
            }
        }

        return approve;
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

    private void Add(Socket client)
    {
        var index = 0;

        if (Connections.Count < highIndex)
        {
            // Procura por um slot que não está sendo usado.
            for (var i = 1; i <= highIndex; i++)
            {
                if (!Connections.ContainsKey(i))
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

        var uniqueKey = new KeyGenerator().GetUniqueKey();

        var connection = new Connection(index, client, uniqueKey);
        connection.OnDisconnect += OnDisconnect;

        Connections.Add(index, connection);
    }
}