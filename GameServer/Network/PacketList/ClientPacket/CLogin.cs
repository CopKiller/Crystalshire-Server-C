using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Network.PacketList.ServerPacket;
using GameServer.Server.Authentication;
using SharedLibrary.Client;
using SharedLibrary.Network;
using SharedLibrary.Util;

namespace GameServer.Network.PacketList.ClientPacket;

public sealed class CLogin : IRecvPacket
{
    public void Process(byte[] buffer, IConnection connection)
    {
        var msg = new ByteBuffer(buffer);
        var username = msg.ReadString();
        var uniqueKey = msg.ReadString();

        if (username.Length > 0 && uniqueKey.Length > 0)
        {
            var authenticator = new WaitingUserAuthentication()
            {
                Username = username,
                UniqueKey = uniqueKey,
                Connection = connection
            };

            authenticator.Authenticate();
        }
    }
}