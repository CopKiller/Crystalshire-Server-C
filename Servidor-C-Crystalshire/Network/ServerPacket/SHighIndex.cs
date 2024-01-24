using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.ServerPacket
{
    public sealed class SHighIndex : SendPacket
    {
        public SHighIndex(int highIndex)
        {
            msg.Write((int)OpCode.SendPacket[GetType()]);
            msg.Write(highIndex);
        }
    }
}
