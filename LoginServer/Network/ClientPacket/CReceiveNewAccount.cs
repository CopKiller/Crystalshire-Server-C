using SharedLibrary.Network;
using SharedLibrary.Network.Interface;

namespace LoginServer.Network.ClientPacket
{
    public sealed class CReceiveNewAccount : IRecvPacket  {
        public void Process(byte[] buffer, IConnection connection) {

            var msg = new ByteBuffer(buffer);

            var name = msg.ReadString();

            var pass = msg.ReadString();

            var code = msg.ReadString();

            //if (name.Length <= 3 || )
            //{

            //}

            //new CReceiveNewAccount().Send(connection);
        }
    }
}