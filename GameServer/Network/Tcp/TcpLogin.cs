using GameServer.Communication;
using SharedLibrary.Util;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Network.Tcp
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

        public void SendPing()
        {
            if (Connection != null)
            {
                Connection.SendPing();
            }
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
            try
            {
                if (accept && server.Poll(0, SelectMode.SelectRead))
                {
                    // Desconecta a conexão anterior, se existir.
                    Connection?.Disconnect();

                    var client = server.Accept();
                    var ipAddress = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();

                    Connection = new Connection(0, client, ipAddress)
                    {
                        Authenticated = true
                    };

                    ChangeState();
                }
            }
            catch (SocketException ex)
            {
                // Lidar com exceções relacionadas a sockets
                Global.WriteLog(LogType.System, $"Login Socket Exception: {ex.Message}", ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                // Lidar com outras exceções
                Global.WriteLog(LogType.System, $"Login Exception: {ex.Message}", ConsoleColor.Red);
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

        // Exibe a alteração no log quando o estado de conexão é alterado.
        private void ChangeState()
        {
            if (Connected != lastState)
            {

                if (Connected)
                {
                    Global.WriteLog(LogType.System, "Login Server is connected...", ConsoleColor.Green);
                }
                else
                {
                    Global.WriteLog(LogType.System, "Login Server is disconnected...", ConsoleColor.Red);
                }

                lastState = Connected;
            }
        }
    }
}
