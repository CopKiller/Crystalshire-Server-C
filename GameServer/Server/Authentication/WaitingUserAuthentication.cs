using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Network.PacketList.ServerPacket;
using GameServer.Server.PlayerData;
using SharedLibrary.Client;
using SharedLibrary.Util;

namespace GameServer.Server.Authentication
{
    public sealed class WaitingUserAuthentication
    {
        public IConnection Connection { get; set; }
        public string Username { get; set; }
        public string UniqueKey { get; set; }

        public void Authenticate()
        {
            var user = new WaitingUserData();

            if (Validate())
            {
                // Sai do método quando o login está duplicado.
                if (IsLoginDuplicated())
                {
                    // Remove os dados da lista obrigando um novo login.
                    WaitingUserData.Remove(user.ListIndex);
                }
                else
                {
                    user = WaitingUserData.FindUser(UniqueKey);

                    // Depois de conferido, adiciona na lista de usuários.
                    Authentication.Add(user, Connection);

                    // Remove da lista já que não é mais necessário.
                    WaitingUserData.Remove(user.ListIndex);

                    SendData();

                    Global.WriteLog(LogType.Player, $"Authenticated user {Username} {UniqueKey}", ConsoleColor.Green);
                }
            }
            else
            {
                Disconnect(Connection, ClientMessages.Connection);
                return;
            }
        }

        // Carrega os chars e classes do usuário.
        private async void SendData()
        {
            var character = new Character();

            Authentication.Players[Connection.Index].Players = await character.GetAccountCharacters(Authentication.Players[Connection.Index].AccountEntityId);

            if (Authentication.Players[Connection.Index].Players == null)
            {
                Global.WriteLog(LogType.Player, $"Error loading characters from {Username}", ConsoleColor.Red);
                var msg = new SAlertMsg(ClientMessages.Connection);
                msg.Send(Connection);
                return;
            }

            // Envia os chars para o usuário.
            var msgPlayerChars = new SPlayerChars(Authentication.Players[Connection.Index].Players);
            msgPlayerChars.Send(Authentication.Players[Connection.Index].Connection);

            // Envia as classes para o usuário.
            var msgClasses = new SClassesData();
            msgClasses.Send(Connection);
        }

        private bool IsLoginDuplicated()
        {

            // Verifica se o usuário já está conectado.
            var pData = Authentication.FindByUsername(Username);

            if (pData != null)
            {
                // Desconecta e envia a mensagem de login duplicado.
                Disconnect(Connection, ClientMessages.Connection);
                Connection.Disconnect();

                // Desconecta e envia a mensagem de tentativa de login.
                Disconnect(pData.Connection, ClientMessages.Connection);

                return true;
            }

            return false;
        }

        private bool Validate()
        {
            var user = new WaitingUserData();
            var isValid = false;

            if (!string.IsNullOrEmpty(UniqueKey))
            {
                user = WaitingUserData.FindUser(UniqueKey);

                if (user != null)
                {
                    if (string.CompareOrdinal(user.Username.ToLower(), Username.ToLower()) == 0)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        private void Disconnect(IConnection connection, ClientMessages alertMessageType)
        {
            var msg = new SAlertMsg(alertMessageType);
            msg.Send(connection);

            connection.Disconnect();
        }
    }
}
