
using GameServer.Communication;
using SharedLibrary.Util;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Network
{
    public sealed class TcpLogin
    {
        public int Port { get; set; }

        public bool Connected
        {
            get
            {
                if (Connection != null)
                {
                    return Connection.Connected;
                }

                return false;
            }
        }

        public string IpAddress { get; set; }

        private bool lastState;
        private bool accept;
        private Socket server;

        /// <summary>
        /// Conexão única para o Login Server.
        /// </summary>
        private Connection Connection;

        public TcpLogin()
        {

        }

        public void Disconnect()
        {
            if (Connection != null)
            {
                Connection.Disconnect();
                ChangeState();
            }
        }

        public void InitServer()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ExclusiveAddressUse = true;
            server.Bind(new IPEndPoint(IPAddress.Any, Port));
            server.Listen(1);

            accept = true;
        }

        public void AcceptClient()
        {
            if (accept)
                if (server.Poll(0, SelectMode.SelectRead))
                {
                    // Se estiver pedindo uma nova conexão.
                    // Desconecta para refazer a conexão.
                    if (Connection != null)
                    {
                        if (Connection.Connected)
                        {
                            Connection.Disconnect();
                        }
                    }

                    var client = server.Accept();

                    var ipAddress = client.RemoteEndPoint.ToString();
                    IpAddress = IpAddress.Remove(IpAddress.IndexOf(':'));

                    Connection = new Connection(0, client, IpAddress)
                    {
                        Authenticated = true
                    };

                    ChangeState();
                }
        }

        public void Stop()
        {
            accept = false;
            server.Close();
        }

        public void ReceiveData()
        {
            if (Connection != null)
            {
                if (Connection.Connected)
                {
                    Connection.ReceiveData();
                }
            }
        }

        /// <summary>
        /// Exibe a alteração no log quando o estado de conexão é alterado.
        /// </summary>
        private void ChangeState()
        {
            if (Connected != lastState)
            {

                if (Connected)
                {
                    Global.WriteLog(LogType.System, "Login Server is connected", ConsoleColor.Green);
                }
                else
                {
                    Global.WriteLog(LogType.System, "Login Server is disconnected", ConsoleColor.Red);
                }

                lastState = Connected;
            }
        }
    }
}
