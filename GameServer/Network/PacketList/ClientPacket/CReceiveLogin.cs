using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Network.PacketList.ServerPacket;
using SharedLibrary.Client;
using SharedLibrary.Network;
using SharedLibrary.Util;

namespace GameServer.Network.PacketList.ClientPacket;

public sealed class CReceiveLogin : IRecvPacket
{
    public void Process(byte[] buffer, IConnection connection)
    {
        var msg = new ByteBuffer(buffer);

        var login = msg.ReadString();
        var uniqueKey = msg.ReadString();

        // Verifica se o login e key estão vazios.
        if (string.IsNullOrEmpty(uniqueKey) || string.IsNullOrEmpty(login))
        {
            new SAlertMsg(ClientMessages.WrongPass, ClientMenu.MenuLogin).Send(connection);
        }

        Global.WriteLog(LogType.Player, $"UniqueKey: {uniqueKey}", ConsoleColor.Blue);

        // Realizar o login no Game Server após receber a conta do login server.
        //var account = DatabaseStartup.Authenticate(login, password);

        //account.Wait();
        //var result = account.Result;

        //if (!result.Success)
        //{
        //    new SAlertMsg(result.ClientMessages, ClientMenu.MenuLogin).Send(connection);
        //}
        //else
        //{
        // Envia a chave unica para o cliente.
        //new SAlertMsg(result.ClientMessages, ClientMenu.MenuLogin,customMsg: connection.GetUniqueKey()).Send(connection);


        // Envia os dados do usuario para o game server.
        //SendUserData(result.Entity);
        // Envia a chave unica para o cliente.
        //SendLoginToken();
    }

    //Busca o index do player para desconectá-lo
    //int playerIndex = Connection.Connections.FirstOrDefault(x => x.Value == (Connection)connection).Key;

    //Desconecta o player
    //Connection.Connections[playerIndex].Disconnect();
}