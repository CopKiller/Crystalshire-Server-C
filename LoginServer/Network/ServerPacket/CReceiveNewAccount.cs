
using SharedLibrary.Network;

namespace LoginServer.Network.ServerPacket
{
    public sealed class CReceiveNewAccount : SendPacket
    {

        public CReceiveNewAccount()
        {
            msg = new ByteBuffer();
            msg.Write(OpCode.SendPacket[GetType()]);
        }
    }
}