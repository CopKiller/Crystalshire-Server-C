using LoginServer.Communication;
using SharedLibrary.Network;

namespace LoginServer.Network.PacketList.GamePacket
{
    public sealed class SPing
    {
        public void Send()
        {
            var msg = new ByteBuffer();
            msg.Write(OpCode.SendPacket[GetType()]);

            Global.SendGameServerPacket?.Invoke(msg);
        }
    }
}
