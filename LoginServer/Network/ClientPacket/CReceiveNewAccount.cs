using SharedLibrary.Client;
using LoginServer.Database;
using LoginServer.Network.ServerPacket;
using SharedLibrary.Network;
using SharedLibrary.Network.Interface;
using LoginServer.Server;

namespace LoginServer.Network.ClientPacket;

public sealed class CReceiveNewAccount : IRecvPacket
{
    public void Process(byte[] buffer, IConnection connection)
    {
        var msg = new ByteBuffer(buffer);

        var name = msg.ReadString();

        var password = msg.ReadString();

        var email = msg.ReadString();


        var result = DatabaseStartup.AdicionarConta(name, password, email);

        var resultIsComplete = result.Result;

        new SAlertMsg(resultIsComplete.ClientMessages, ClientMenu.MenuLogin).Send(connection);

        //Desconecta o player
        connection.Disconnect();
    }
}