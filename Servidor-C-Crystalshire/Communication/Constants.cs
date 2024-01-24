namespace GameServer.Communication;

public class Constants
{
    /// <summary>
    ///     Tempo limite de leitura em microsegundos.
    /// </summary>
    public const int ReceiveTimeOut = 15 * 1000 * 1000;

    /// <summary>
    ///     Tempo limite de envio em microsegundos.
    /// </summary>
    public const int SendTimeOut = 15 * 1000 * 1000;

    /// <summary>
    ///     Tempo entre cada ping.
    /// </summary>
    public const int PingTime = 15000;

    /// <summary>
    ///     Dados de Conexão.
    /// </summary>
    public const string Ip = "127.0.0.1";

    // Player
    public const int GameServerPort = 7001;
    public const int LoginServerPort = 7002;
    // Game server to receive data from login server.
    public const int GameServerReceiveLoginPort = 7003;

    public const int MaxConnections = 5000;
    public const int CheckAccessTime = 3500;
    public const int IpMaxAttempt = 15;
    public const int IpMaxAccessCount = 10;
    public const int IpLifetime = 120000;

    public const int IpBlockTime = 600000;

    /// <summary>
    /// Tempo limite em segundos de uma conexão no sistema.
    /// </summary>
    public const int ConnectionTimeOut = 4;
}