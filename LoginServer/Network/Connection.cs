using System.Net.Sockets;
using LoginServer.Communication;
using SharedLibrary.Network;
using SharedLibrary.Network.Interface;
using SharedLibrary.Util;

namespace LoginServer.Network;

public sealed class Connection : IConnection
{
    private readonly Socket Client;

    private readonly ByteBuffer msg;

    public Connection(Socket tcpClient, string ipAddress, string uniqueKey)
    {
        msg = new ByteBuffer();

        IpAddress = ipAddress;
        UniqueKey = uniqueKey;

        Client = tcpClient;

        Connected = true;
        Add(this);
    }

    public static Dictionary<int, Connection> Connections { get; set; } = new();

    // Maior indice da lista de usuários.
    public static int HighIndex { get; set; }

    public string UniqueKey { get; set; }
    public bool Connected { get; set; }

    public string IpAddress { get; set; }

    public void ReceiveData()
    {
        if (Client == null) return;

        // Verifica se há dados disponíveis sem bloquear.
        if (Client.Poll(-1, SelectMode.SelectRead))
        {
            try
            {
                if (Client.Available > 0)
                {
                    var size = Client.Available;
                    var buffer = new byte[size];

                    // Recebe os dados disponíveis.
                    Client.Receive(buffer, size, SocketFlags.None);

                    // Escreve o buffer.
                    msg.Write(buffer);

                    var pSize = msg.ReadInt32(false);

                    // Enquanto a mensagem não chegar por completo, lê os dados e adiciona no buffer.
                    while (msg.Count() - 4 < pSize)
                        if (Client.Available > 0)
                        {
                            buffer = new byte[Client.Available];

                            // Recebe os dados disponíveis.
                            Client.Receive(buffer, Client.Available, SocketFlags.None);

                            // Escreve o buffer.
                            msg.Write(buffer);
                        }
                }
                else
                {
                    // Cliente desconectado
                    Disconnect();
                    return;
                }
            }
            catch (SocketException ex)
            {
                Global.WriteLog(LogType.System, $"Receive Data Error: Class {GetType().Name}", ConsoleColor.Red);
                Global.WriteLog(LogType.System, $"Message: {ex.Message}", ConsoleColor.Red);
                Disconnect();
                return;
            }
        }
        else
        {
            // Cliente desconectado
            Disconnect();
            return;
        }

        var pLength = 0;

        Global.WriteLog(LogType.Debug, $"Receive -> {IpAddress} {GetType().Name} Buffer Size: {msg.Length()}",
            ConsoleColor.Black);

        if (msg.Length() >= 4)
        {
            pLength = msg.ReadInt32(false);

            if (pLength < 0) return;
        }

        while (pLength > 0 && pLength <= msg.Length() - 4)
        {
            if (pLength <= msg.Length() - 4)
            {
                // Remove the first packet (Size of Packet).
                msg.ReadInt32();

                // Remove the header.
                var header = msg.ReadInt32();
                // Decrease 4 bytes of header.
                pLength -= 4;

                if (OpCode.RecvPacket.ContainsKey(header))
                {
                    ((IRecvPacket)Activator.CreateInstance(OpCode.RecvPacket[header])).Process(msg.ReadBytes(pLength),
                        this);
                    Global.WriteLog(LogType.System, $"Recebido: {Enum.GetName(typeof(Packet), header)}",
                        ConsoleColor.Black);
                }
                else
                {
                    Global.WriteLog(LogType.System, $"Header: {header} was not found", ConsoleColor.Red);
                }
            }

            pLength = 0;

            if (msg.Length() >= 4)
            {
                pLength = msg.ReadInt32(false);

                if (pLength < 0) return;
            }
        }

        msg.Clear();
    }

    public void Send(ByteBuffer msg, string className)
    {
        var buffer = new byte[msg.Length() + 4];
        var values = BitConverter.GetBytes(msg.Length());

        Array.Copy(values, 0, buffer, 0, 4);
        Array.Copy(msg.ToArray(), 0, buffer, 4, msg.Length());

        if (Client.Poll(Constants.SendTimeOut, SelectMode.SelectWrite))
            try
            {
                Client.Send(buffer, SocketFlags.None);
            }
            catch (SocketException ex)
            {
                if (ex.ErrorCode > 0)
                {
                }

                Global.WriteLog(LogType.System, $"Send Data Error: Class {className}", ConsoleColor.Red);
                Global.WriteLog(LogType.System, $"Message: {ex.Message}", ConsoleColor.Red);
                Disconnect();
            }
        else
            Disconnect();
    }

    public static bool Remove(int index)
    {
        var connection = Connections[index];

        if (Connections.ContainsKey(index))
        {
            Connections.Remove(index);
            connection.Connected = false;

            return true;
        }

        return false;
    }

    private static void Add(Connection connection)
    {
        if (Connections.Count == 0)
        {
            ++HighIndex;
            Connections.Add(HighIndex, connection);
        }
    }

    public bool Disconnect()
    {
        if (Connected)
        {
            Client.Close();
            Connected = false;

            Global.WriteLog(LogType.System, $"{IpAddress} Key {UniqueKey} is disconnected", ConsoleColor.Red);

            return true;
        }

        return false;
    }
}