using LoginServer.Communication;
using LoginServer.Network.PacketList.GamePacket;
using SharedLibrary.Network;
using SharedLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Network.Tcp
{
    public sealed class TcpGameServer
    {
        public int GamePort { get; set; }
        public string GameIpAddress { get; set; }
        public bool Connected { get; private set; }

        // Tempo entre cada tentativa de conexão.
        private const int ConnectTime = 5000;

        private bool disposed { get; set; }

        private Socket client;
        private bool lastState;
        private int tick;

        private int pingTick;
        private SPing ping;

        public void InitClient()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true
            };

            ping = new SPing();

            tick = Environment.TickCount;
        }

        public void Connect()
        {

            if (Environment.TickCount >= tick + ConnectTime)
            {
                tick = Environment.TickCount;

                if (!Connected)
                {
                    if (disposed)
                    {
                        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                        {
                            NoDelay = true
                        };

                        disposed = false;
                    }

                    try
                    {
                        client.Connect(GameIpAddress, GamePort);
                        Connected = true;
                    }
                    catch (SocketException ex)
                    {
                        // SocketException foi definido para o campo não ficar vazio.                  
                        if (ex.SocketErrorCode == SocketError.Success)
                        {
                            Global.WriteLog(LogType.System, $"Connect Error: Class {GetType().Name}", ConsoleColor.Red);
                            Global.WriteLog(LogType.System, $"Message: {ex.SocketErrorCode.ToString()}", ConsoleColor.Red);
                        }

                        Disconnect();
                    }
                }

                ChangeState();
            }
        }

        public void Disconnect()
        {
            client.Close();
            Connected = false;
            disposed = true;

            ChangeState();
        }

        public void Send(ByteBuffer msg)
        {
            var buffer = new byte[msg.Length() + 4];
            var values = BitConverter.GetBytes(msg.Length());

            Array.Copy(values, 0, buffer, 0, 4);
            Array.Copy(msg.ToArray(), 0, buffer, 4, msg.Length());

            if (client.Poll(Constants.SendTimeOut, SelectMode.SelectWrite))
            {
                try
                {
                    client.Send(buffer, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    // SocketException foi definido para o campo não ficar vazio.                  
                    if (ex.SocketErrorCode == SocketError.Success)
                    {
                        Global.WriteLog(LogType.System, $"Send Data Error: Class {GetType().Name}", ConsoleColor.Red);
                        Global.WriteLog(LogType.System, $"Message: {ex.SocketErrorCode.ToString()}", ConsoleColor.Red);
                    }

                    Disconnect();
                }
            }
            else
            {
                Disconnect();
            }
        }

        /// Exibe a alteração no log quando o estado de conexão é alterado.
        private void ChangeState()
        {
            if (Connected != lastState)
            {

                if (Connected)
                {
                    Global.WriteLog(LogType.System, "Game Server is connected", ConsoleColor.Green);
                }
                else
                {
                    Global.WriteLog(LogType.System, "Game Server is disconnected", ConsoleColor.Red);
                }

                lastState = Connected;
            }
        }

        public void SendPing()
        {
            if (Environment.TickCount >= pingTick + Constants.PingTime)
            {
                if (Connected)
                {
                    pingTick = Environment.TickCount;
                    ping.Send();
                }
            }
        }
    }
}
