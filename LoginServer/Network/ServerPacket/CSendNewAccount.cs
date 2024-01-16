
using SharedLibrary.Network;

namespace LoginServer.Network.ServerPacket
{
    public sealed class CSendNewAccount : SendPacket
    {

        public CSendNewAccount()
        {
            //msg = new ByteBuffer();

            //var name = msg.ReadString();

            //var pass = msg.ReadString();

            //var code = msg.ReadString();


            msg.Write(OpCode.SendPacket[GetType()]);
        }
    }
}