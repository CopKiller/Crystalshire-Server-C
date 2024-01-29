using GameServer.Communication;
using GameServer.Network;
using GameServer.Network.Tcp;
using GameServer.Server.Data;
using SharedLibrary.Util;

namespace GameServer.Server;

public class DataServer
{

    Task t;

    private int count;
    private TcpServer Server;
    private TcpLogin Login;
    IpFiltering IpFiltering;

    private int tick;
    public Action<int> UpdateUps;
    private int ups;
    public bool ServerRunning { get; set; } = true;

    public void InitializeServer()
    {
        //Inicializações estáticas
        Global.InitLogs();

        // Inicializa a lista de IPs bloqueados.
        IpBlockList.Initialize();
        IpBlockList.Enabled = true;
        IpBlockList.IpBlockTime = Constants.IpBlockTime;

        // Inicializa o filtro de IP.
        IpFiltering = new IpFiltering()
        {
            CheckAccessTime = Constants.CheckAccessTime,
            IpLifetime = Constants.IpLifetime,
            IpMaxAccessCount = Constants.IpMaxAccessCount,
            IpMaxAttempt = Constants.IpMaxAttempt
        };

        // Carrega as configurações.
        Global.WriteLog(LogType.System, "Loading configuration...", ConsoleColor.Yellow);
        var configuration = new Configuration();
        configuration.InitConfiguration();

        // Inicializa o servidor TCP.
        Server = new TcpServer() { Port = Constants.GameServerPort, IpFiltering = IpFiltering };
        Server.InitServer();
        Global.WriteLog(LogType.System, "TCP Protocol Initialized...", ConsoleColor.Green);

        // Inicializa o servidor de login.
        Login = new TcpLogin() { Port = Constants.GameServerReceiveLoginPort };
        Login.InitServer();

        // Inicializa a indexação dos índices com os pacotes.
        OpCode.InitOpCode();

        // Inicializa o banco de dados.
        Global.InitDatabase();

        Global.WriteLog(LogType.System, "Waiting For Login Server Connection...", ConsoleColor.Yellow);

        t = new Task(ServerLoop);
        t.Start();
    }

    public void ServerLoop()
    {

        // Inicializa o loop do servidor.
        while (ServerRunning)
        {
            if (Server != null)
            {
                Login.AcceptClient();
                Login.ReceiveData();
                Login.SendPing();

                Server.AcceptClient();
                Server.ReceiveData();
                Server.SendPing();

                Server.CheckConnectionTimeOut();

                IpFiltering.RemoveExpiredIpAddress();
                IpBlockList.RemoveExpiredIpAddress();

                CountUps();

                Thread.Sleep(1);
            }
        }

        StopServer();
        Environment.Exit(0);
    }

    public void StopServer()
    {
        Server.Stop();
        Login.Stop();

        Authentication.Authentication.Clear();

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
}