using SharedLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.PacketList.ServerPacket
{
    public sealed class SPing: SendPacket
    {
        public SPing()
        {
            var msg = new ByteBuffer();
            msg.Write((int)OpCode.SendPacket[GetType()]);
        }
    }
}
