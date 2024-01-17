using System.Net;
using System.Net.Sockets;
using LoginServer.Communication;
using SharedLibrary.Communication;
using SharedLibrary.Util;

namespace LoginServer.Network;

public sealed class TcpServer
{
    private bool accept;
    private Socket server;

    public TcpServer()
    {
    }

    public TcpServer(int port)
    {
        Port = port;
    }

    public int Port { get; set; }

    public async Task InitServer()
    {
        server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        server.Bind(new IPEndPoint(IPAddress.Any, Port));
        server.Listen(10);

        Global.WriteLog(LogType.System, "TCP Protocol Initialized...", ConsoleColor.Green);

        accept = true;

        await Task.CompletedTask;
    }

    public void AcceptClient()
    {
        if (accept)
            if (server.Poll(0, SelectMode.SelectRead))
            {
                var client = server.Accept();
                var ipAddress = client.RemoteEndPoint.ToString();

                if (IsValidIpAddress(ipAddress))
                {
                    var uniqueKey = new KeyGenerator().GetUniqueKey();

                    new Connection(client, ipAddress, uniqueKey);
                    Global.WriteLog(LogType.System, $"{ipAddress} Key {uniqueKey} is connected", ConsoleColor.Green);
                }
                else
                {
                    client.Close();
                    Global.WriteLog(LogType.System, $"Hacking Attempt: Invalid IpAddress {ipAddress}",
                        ConsoleColor.Red);
                }
            }
    }


    public void Stop()
    {
        accept = false;
        server.Close();
    }

    private bool IsValidIpAddress(string ipAddress)
    {
        const int IpAddressArraySplit = 4;
        const int Last = 3;

        if (string.IsNullOrWhiteSpace(ipAddress) || string.IsNullOrEmpty(ipAddress)) return false;

        var values = ipAddress.Split('.');
        if (values.Length != IpAddressArraySplit) return false;

        // Retira o número da porta.
        values[Last] = values[Last].Remove(values[Last].IndexOf(':'));

        return values.All(r => byte.TryParse(r, out var parsing));
    }
}