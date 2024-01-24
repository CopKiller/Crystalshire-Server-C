﻿
using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Network.ServerPacket;
using SharedLibrary.Client;
using SharedLibrary.Network;
using SharedLibrary.Util;

namespace GameServer.Network.ClientPacket;

public sealed class CReceiveLogin : IRecvPacket
{
    public void Process(byte[] buffer, IConnection connection)
    {
        var msg = new ByteBuffer(buffer);

        var uniqueKey = msg.ReadString();

        // Verifica se o login e key estão vazios.
        if (string.IsNullOrEmpty(uniqueKey))
        {
            new SAlertMsg(ClientMessages.WrongPass, ClientMenu.MenuLogin).Send(connection);
        }

        Global.WriteLog(LogType.Player, $"UniqueKey: {uniqueKey}", ConsoleColor.Blue);

        // Processar a chave unica e buscar se houve autenticação do login server.
        //var UniqueKey = msg.ReadString();

        // Verificação da uniqueKey para obtenção do login.

        // Busca a conta no banco de dados.

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