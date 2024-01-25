using SharedLibrary.Network;
using GameServer.Server.Authentication;
using GameServer.Network.Interface;


namespace GameServer.Network.PacketList.LoginPacket
{
    public sealed class CUserData : IRecvPacket
    {
        public void Process(byte[] buffer, IConnection connection)
        {
            var msg = new ByteBuffer(buffer);

            WaitingUserData.Add(
                new WaitingUserData()
                {
                    AccountId = msg.ReadInt32(),
                    Username = msg.ReadString(),
                    UniqueKey = msg.ReadString(),
                }
            );
        }
    }
}
