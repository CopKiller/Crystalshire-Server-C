using LoginServer.Communication;
using LoginServer.Database;
using LoginServer.Network;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Network;
using SharedLibrary.Util;

namespace LoginServer.Server;

public class DataServer
{
    public Action<int> UpdateUps { get; set; }

    private TcpServer Server;

    private TcpGameServer GameServer;

    public IpFiltering IpFiltering { get; set; }

    // Fps Variaveis.
    private int count;
    private int tick;
    private int ups;

    public void InitServer()
    {

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

        DatabaseStartup.Configure();

        GameServer = new TcpGameServer()
        {
            GameIpAddress = Constants.IpGameServer,
            GamePort = Constants.GameServerReceiveLoginPort
        };

        GameServer.InitClient();
        Global.WriteLog(LogType.System, "Trying to connect to Game Server...", ConsoleColor.Magenta);

        Server = new TcpServer() { Port = Constants.LoginServerPort, IpFiltering = IpFiltering };
        Server.InitServer();

        // Delegate 
        Global.SendGameServerPacket = SendGameServerData;

        Global.WriteLog(LogType.System, "Login Server started", ConsoleColor.Green);

        ServerLoop();
    }

    public void ServerLoop()
    {
        while (true)
            if (Server != null)
            {
                GameServer.Connect();

                Server.AcceptClient();
                Server.ProcessClients();

                IpBlockList.RemoveExpiredIpAddress();
                IpFiltering.RemoveExpiredIpAddress();

                CountUps();
            }
    }

    public void StopServer()
    {
        Server.Stop();

        GameServer.Disconnect();
        GameServer = null;

        IpFiltering.Clear();

        IpBlockList.Clear();

        Global.CloseLog();
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