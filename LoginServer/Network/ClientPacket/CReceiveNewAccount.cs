using Database.Client;
using LoginServer.Database;
using LoginServer.Network.ServerPacket;
using SharedLibrary.Network;
using SharedLibrary.Network.Interface;

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

        new SAlertMsg(resultIsComplete.ClientMSG, ClientMenu.MenuLogin).Send(connection);

        //Busca o index do player para desconectá-lo
        int playerIndex = Connection.Connections.FirstOrDefault(x => x.Value == (Connection)connection).Key;

        //Desconecta o player
        Connection.Connections[playerIndex].Disconnect();
    }
}