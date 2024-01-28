using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.PacketList.ServerPacket
{
    public sealed class SInGame : SendPacket
    {
        public SInGame()
        {
            msg.Write((int)OpCode.SendPacket[GetType()]);
        }
    }
}
