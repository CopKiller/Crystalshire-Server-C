using LoginServer.Communication;
using SharedLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Network.GamePacket
{
    public sealed class SSendUserData
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string UniqueKey { get; set; }

        public void Send()
        {
            var msg = new ByteBuffer();

            msg.Write(OpCode.SendPacket[GetType()]);
            msg.Write(AccountId);
            msg.Write(Username);
            msg.Write(UniqueKey);

            Global.SendGameServerPacket?.Invoke(msg);
        }
    }
}
