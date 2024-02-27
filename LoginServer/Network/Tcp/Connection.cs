using Database.Entities.Account;
using LoginServer.Communication;
using LoginServer.Network.PacketList;
using SharedLibrary.Network;
using SharedLibrary.Network.Interface;
using SharedLibrary.Util;
using System.Net.Sockets;

namespace LoginServer.Network.Tcp;

public sealed class Connection : IConnection
{

    public Action<int> OnDisconnect { get; set; }

    // Chave única de identificação no sistema.
    public string UniqueKey { get; set; }

    // Id de indice de conexão.
    public int Index { get; set; }

    // Tempo de conexão em segundos.
    public int ConnectedTime { get; set; }

    public bool Connected { get; private set; }

    public string IpAddress { get; set; }

    private Socket Client;
    private ByteBuffer msg;

    private bool lastState;
    private int timeTick;
    private const int OneSecond = 1000;

    public string GetUniqueKey()
    {
        return UniqueKey;
    }

    public Connection(int index, Socket tcpClient, string uniqueKey)
    {
        timeTick = Environment.TickCount;
        msg = new ByteBuffer();

        Index = index;
        Client = tcpClient;
        Client.NoDelay = true;

        UniqueKey = uniqueKey;
        Connected = true;

        IpAddress = tcpClient.RemoteEndPoint.ToString();
        ChangeState();
    }

    public void Disconnect()
    {
        Connected = false;
        Client.Close();
        ChangeState();
        msg.Clear();

        OnDisconnect?.Invoke(Index);
    }

    public void ReceiveData()
    {
        if (Client == null) return;

        if (Client.Available > 0)
        {
            var size = Client.Available;
            var buffer = new byte[size];

            // Verifica se há dados disponíveis sem bloquear.
            if (Client.Poll(Constants.ReceiveTimeOut, SelectMode.SelectRead))
            {
                try
                {
                    // Recebe os dados disponíveis.
                    Client.Receive(buffer, size, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    Global.WriteLog(LogType.System, $"Receive Data Error: Class {GetType().Name}", ConsoleColor.Red);
                    Global.WriteLog(LogType.System, $"Message: {ex.Message}", ConsoleColor.Red);
                    Disconnect();
                    return;
                }

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
                        ((IRecvPacket)Activator.CreateInstance(OpCode.RecvPacket[header])).Process(msg.ReadBytes(pLength), this);
                        Global.WriteLog(LogType.System, $"Recebido: {Enum.GetName(typeof(ClientPacketEnum), header)}",
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

    public void CountConnectionTime()
    {
        if (Environment.TickCount >= timeTick + OneSecond)
        {
            ConnectedTime++;
            timeTick = Environment.TickCount;
        }
    }

    // Exibe a alteração no log quando o estado de conexão é alterado.
    private void ChangeState()
    {
        if (Connected != lastState)
        {

            if (Connected)
            {
                Global.WriteLog(LogType.Player, $"Index: {Index} Key {UniqueKey} {IpAddress} is connected", ConsoleColor.Green);
            }
            else
            {
                Global.WriteLog(LogType.Player, $"Index: {Index} Key {UniqueKey} {IpAddress} is disconnected", ConsoleColor.Red);
            }

            lastState = Connected;
        }
    }
}