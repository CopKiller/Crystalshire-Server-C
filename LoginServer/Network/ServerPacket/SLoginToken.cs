using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Network.ServerPacket
{
    public sealed class SLoginToken : SendPacket
    {
        public SLoginToken(string uniqueKey)
        {
            msg.Write(OpCode.SendPacket[GetType()]);
            msg.Write(uniqueKey);
        }
    }
}
