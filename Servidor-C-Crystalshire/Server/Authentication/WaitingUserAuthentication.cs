using GameServer.Communication;
using GameServer.Database;
using GameServer.Network.Interface;
using GameServer.Network.ServerPacket;
using SharedLibrary.Client;
using SharedLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// Carrega os dados dos personages e envia as classes.
        /// </summary>
        private async void SendData()
        {
            var chars = await DatabaseStartup.GetAccountCharacters(Authentication.Players[Connection.Index].AccountEntityId);

            var msgPlayerChars = new SPlayerChars(chars);
            msgPlayerChars.Send(Authentication.Players[Connection.Index].Connection);
            

            // Falta criar um carregador de classes, para enviar pro client.
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
