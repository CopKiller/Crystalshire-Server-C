namespace LoginServer.Communication;

public class Constants
{

    // Tempo limite de leitura em microsegundos.
    public const int ReceiveTimeOut = 15 * 1000 * 1000;

    ///     Tempo limite de envio em microsegundos.
    public const int SendTimeOut = 15 * 1000 * 1000;

    ///     Tempo entre cada ping.
    public const int PingTime = 15000;

    public const int ConnectionTimeOut = 4;

    ///     Dados de Conexão.
    public const string Ip = "127.0.0.1";
    public const string IpGameServer = "127.0.0.1";

    // Player
    public const int GameServerPort = 7001;
    public const int LoginServerPort = 7002;

    // Game server to receive data from login server.
    public const int GameServerReceiveLoginPort = 7003;
 

    public const int MaxConnections = 100;
    public const int CheckAccessTime = 3500;
    public const int IpMaxAttempt = 15;
    public const int IpMaxAccessCount = 10;
    public const int IpLifetime = 120000;

    public const int IpBlockTime = 600000;
}