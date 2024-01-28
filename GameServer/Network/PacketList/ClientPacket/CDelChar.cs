using GameServer.Network.Interface;
using GameServer.Server.PlayerData;
using SharedLibrary.Network;

namespace GameServer.Network.PacketList.ClientPacket
{
    public sealed class CDelChar : IRecvPacket
    {
        public void Process(byte[] buffer, IConnection connection)
        {
            var msg = new ByteBuffer(buffer);
            var charSlot = msg.ReadInt32();

            new Character().Exclude(connection.Index, charSlot);
        }
    }
}
