

using GameServer.Communication;
using GameServer.Data.Configuration;
using GameServer.Database;
using GameServer.Server.Authentication;
using SharedLibrary.Util;

namespace GameServer.Network;

public class DataServer
{
    private int count;
    private TcpServer Server;
    private TcpLogin Login;
    IpFiltering IpFiltering;

    private int tick;
    public Action<int> UpdateUps;
    private int ups;
    public bool ServerRunning { get; set; } = true;

    public void InitServer()
    {
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
        Global.WriteLog(LogType.System, "Waiting For Login Server Connection...", ConsoleColor.Red);

        // Inicializa a indexação dos índices com os pacotes.
        OpCode.InitOpCode();

        // Inicializa o banco de dados.
        DatabaseStartup.Configure();

        // Inicializa o loop do servidor.
        while (true)
            if (Server != null)
            {
                ServerLoop();

                Thread.Sleep(1);

                if (!ServerRunning)
                {
                    StopServer();
                    Environment.Exit(0);
                }
            }
    }

    public void ServerLoop()
    {
        // Aceita a conexão do servidor de login.
        Login.AcceptClient();
        // Recebe os dados.
        Login.ReceiveData();


        Server.AcceptClient();
        Server.ReceiveData();
        Server.CheckConnectionTimeOut();

        IpFiltering.RemoveExpiredIpAddress();
        IpBlockList.RemoveExpiredIpAddress();

        CountUps();
    }

    public void StopServer()
    {
        Server.Stop();
        Login.Stop();

        Authentication.Clear();

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