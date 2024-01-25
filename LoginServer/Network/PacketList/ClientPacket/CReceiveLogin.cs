using SharedLibrary.Client;
using LoginServer.Database;
using LoginServer.Network.ServerPacket;
using SharedLibrary.Network;
using SharedLibrary.Network.Interface;
using LoginServer.Communication;
using SharedLibrary.Util;
using LoginServer.Network.GamePacket;
using Database.Entities.Account;
using LoginServer.Network.Tcp;
using System.Diagnostics;

namespace LoginServer.Network.ClientPacket;

public sealed class CReceiveLogin : IRecvPacket
{
    IConnection _Connection;
    public void Process(byte[] buffer, IConnection connection)
    {
        _Connection = connection;
        var msg = new ByteBuffer(buffer);

        var login = msg.ReadString();

        Global.WriteLog(LogType.Player, $"Login: {login}", ConsoleColor.Blue);

        var password = msg.ReadString();

        Global.WriteLog(LogType.Player, $"Password: {password}", ConsoleColor.Blue);

        var account = DatabaseStartup.Authenticate(login, password);

        account.Wait();
        var result = account.Result;

        if (!result.Success)
        {
            new SAlertMsg(result.ClientMessages, ClientMenu.MenuLogin).Send(connection);
        }
        else
        {

            // Debug
            Global.WriteLog(LogType.Player, $"UniqueKey: {((Connection)_Connection).UniqueKey}", ConsoleColor.Blue);

            // Envia os dados do usuario para o game server.
            SendUserData(result.Entity);
            // Envia a chave unica para o cliente.
            SendLoginToken(login);
        }

        //Desconecta o player
        connection.Disconnect();
    }

    private void SendLoginToken(string Login)
    {
        var msg = new SLoginToken(Login, ((Connection)_Connection).UniqueKey);
        msg.Send(_Connection);
    }

    private void SendUserData(AccountEntity account)
    {
        var msg = new SSendUserData()
        {
            AccountId = account.Id,
            Username = account.Login,
            UniqueKey = ((Connection)_Connection).UniqueKey
        };

        msg.Send();
    }
}