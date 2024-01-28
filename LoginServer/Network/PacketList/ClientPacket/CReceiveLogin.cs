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

        var password = msg.ReadString();

        DatabaseStartup.Authenticate(connection, login, password);
    }
}