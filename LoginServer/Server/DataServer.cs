using LoginServer.Communication;
using LoginServer.Database;
using LoginServer.Network;
using LoginServer.Network.GamePacket;
using LoginServer.Network.Tcp;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Network;
using SharedLibrary.Util;
using static System.Net.Mime.MediaTypeNames;

namespace LoginServer.Server;

public class DataServer
{
    Thread t;

    public Action<int> UpdateUps { get; set; }

    private TcpServer Server;

    private TcpGameServer GameServer;

    public IpFiltering IpFiltering { get; set; }

    // Fps Variaveis.
    private int count;
    private int tick;
    private int ups;

    private bool ServerRunning;

    public void InitializeServer()
    {
        ServerRunning = true;

        //Inicializações estáticas
        Global.InitLogs();

        IpBlockList.Enabled = true;
        IpBlockList.IpBlockTime = Constants.IpBlockTime;
        IpBlockList.Initialize();

        IpFiltering = new IpFiltering()
        {
            CheckAccessTime = Constants.CheckAccessTime,
            IpLifetime = Constants.IpLifetime,
            IpMaxAccessCount = Constants.IpMaxAccessCount,
            IpMaxAttempt = Constants.IpMaxAttempt
        };

        OpCode.InitOpCode();

        GameServer = new TcpGameServer()
        {
            GameIpAddress = Constants.IpGameServer,
            GamePort = Constants.GameServerReceiveLoginPort
        };
        GameServer.InitClient();

        Server = new TcpServer() { Port = Constants.LoginServerPort, IpFiltering = IpFiltering };
        Server.InitServer();

        Global.WriteLog(LogType.System, "TCP Protocol Initialized...", ConsoleColor.Green);

        // Delegate 
        Global.SendGameServerPacket = SendGameServerData;

        Global.WriteLog(LogType.System, "Login Server started...", ConsoleColor.Green);

        DatabaseStartup.Configure();

        Global.WriteLog(LogType.System, "Trying to connect to Game Server...", ConsoleColor.Yellow);

        t = new Thread(ServerLoop);
        t.Start();
    }

    public void ServerLoop()
    {
        while (ServerRunning)
        {
            GameServer.Connect();

            Server.AcceptClient();
            Server.ProcessClients();

            IpBlockList.RemoveExpiredIpAddress();
            IpFiltering.RemoveExpiredIpAddress();

            CountUps();

            Thread.Sleep(1);
        }

        StopServer();
        Environment.Exit(0);
    }

    public void StopServer()
    {
        Server.Stop();

        GameServer.Disconnect();
        GameServer = null;

        IpFiltering.Clear();

        IpBlockList.Clear();

        Global.CloseLog();

        ServerRunning = false;
    }

    private void CountUps()
    {
        if (Environment.TickCount >= tick + 1000)
        {
            ups = count;
            count = 0;
            tick = Environment.TickCount;

            UpdateUps?.Invoke(ups);
        }
        else
        {
            count++;
        }
    }

    private void SendGameServerData(ByteBuffer msg)
    {
        if (GameServer.Connected)
        {
            GameServer.Send(msg);
        }
    }
}