using SharedLibrary.Client;
using LoginServer.Database;
using LoginServer.Network.ServerPacket;
using SharedLibrary.Network;
using SharedLibrary.Network.Interface;
using LoginServer.Communication;
using SharedLibrary.Util;
using LoginServer.Network.GamePacket;
using Database.Entities.Account;

namespace LoginServer.Network.ClientPacket;

public sealed class CReceiveLogin : IRecvPacket
{
    IConnection _Connection;
    public void Process(byte[] buffer, IConnection connection)
    {
        _Connection = connection;
        var msg = new ByteBuffer(buffer);

        var login = msg.ReadString();

        var password = msg.ReadString();


        var account = DatabaseStartup.Authenticate(login, password);

        account.Wait();
        var result = account.Result;

        if (!result.Success)
        {
            new SAlertMsg(result.ClientMessages, ClientMenu.MenuLogin).Send(connection);
        }
        else
        {
            // Envia a chave unica para o cliente.
            new SAlertMsg(result.ClientMessages, ClientMenu.MenuLogin,customMsg: connection.UniqueKey).Send(connection);


            // Envia os dados do usuario para o game server.
            SendUserData(result.Entity);
            // Envia a chave unica para o cliente.
            SendLoginToken();
        }

        //Desconecta o player
        connection.Disconnect();
    }

    private void SendLoginToken()
    {
        var msg = new SLoginToken(((Connection)_Connection).UniqueKey);
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