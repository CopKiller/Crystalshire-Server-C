using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Network.PacketList;
using GameServer.Network.PacketList.ServerPacket;
using SharedLibrary.Network;
using SharedLibrary.Util;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GameServer.Network.Tcp
{
    public sealed class Connection : IConnection
    {
        private readonly Socket Client;

        private readonly ByteBuffer msg;

        public Action<int> OnDisconnect { get; set; }
        public bool Authenticated { get; set; }
        public bool Connected { get; set; }
        public int ConnectedTime { get; private set; }
        public string IpAddress { get; set; }
        public int Index { get; set; }

        private bool lastState;
        private const int OneSecond = 1000;

        private int tick;
        private int time;
        private int pingTick;

        //Ping
        private SPing ping;

        public Connection(int index, Socket tcpClient, string ipAddress)
        {
            msg = new ByteBuffer();

            Client = tcpClient;
            Client.NoDelay = false;

            Index = index;
            Connected = true;
            IpAddress = ipAddress;

            if (index != 0)
            {
                ChangeState();
            }
        }

        public void ReceiveData()
        {
            if (Client == null) return;

            // Verifica se há dados disponíveis sem bloquear.
            if (Client.Available > 0)
            {
                try
                {
                    if (Client.Poll(Constants.ReceiveTimeOut, SelectMode.SelectRead))
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
                        // Cliente desconectado ou sem dados disponíveis
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

                        if (Enum.IsDefined(typeof(ClientPacketEnum), header))
                        {
                            if (OpCode.RecvPacket.ContainsKey((ClientPacketEnum)header))
                            {

                                // Se os dados do usuário estiverem atuenticados, processa dos pacotes.
                                if (Authenticated)
                                {
                                    ((IRecvPacket)Activator.CreateInstance(OpCode.RecvPacket[(ClientPacketEnum)header])).Process(msg.ReadBytes(pLength), this);
                                }
                                // Do contrário, redireciona para o a autenticação do usuário.
                                // Caso algum pacote esteja fora de ordem ou com informações incorretas.
                                // A desconexão irá ocorrer.
                                else
                                {
                                    ((IRecvPacket)Activator.CreateInstance(OpCode.RecvPacket[ClientPacketEnum.AuthLogin])).Process(msg.ReadBytes(pLength), this);
                                }
                            }

                        }
                        else
                        {
                            Global.WriteLog(LogType.System, $"Header: {header} was not found", ConsoleColor.Red);
                            msg.Clear();
                            Disconnect();
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
            {
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
            }
            else
            {
                Disconnect();
            }
        }

        public void CheckConnectionTimeOut()
        {
            if (!Authenticated)
            {
                if (Environment.TickCount >= tick)
                {
                    time++;

                    if (time >= Constants.ConnectionTimeOut)
                    {
                        Disconnect();
                    }

                    tick = Environment.TickCount + OneSecond;
                }
            }
        }

        public void Disconnect()
        {
            Connected = false;
            Client.Close();
            ChangeState();
            msg.Clear();

            OnDisconnect?.Invoke(Index);
        }

        private void ChangeState()
        {
            if (Connected != lastState)
            {

                if (Connected)
                {
                    Global.WriteLog(LogType.Player, $"Index: {Index} {IpAddress} is connected", ConsoleColor.Blue);
                }
                else
                {
                    Global.WriteLog(LogType.Player, $"Index: {Index} {IpAddress} is disconnected", ConsoleColor.Blue);
                }

                lastState = Connected;
            }
        }

        public void SendPing()
        {
            if (Environment.TickCount >= pingTick)
            {
                ping = new SPing();

                if (Connected)
                {
                    ping.Send(this);
                }

                pingTick = Environment.TickCount + Constants.PingTime;
            }
        }
    }
}
