using GameServer.Network.Interface;
using GameServer.Server;
using GameServer.Server.Authentication;
using SharedLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.PacketList.ClientPacket
{
    public sealed class CDelChar : IRecvPacket
    {
        public void Process(byte[] buffer, IConnection connection)
        {
            var msg = new ByteBuffer(buffer);
            var charSlot = msg.ReadInt32();

            var characterData = Authentication.FindByIndex(connection.Index);

            new Character().Exclude(characterData.Index, charSlot, characterData.AccountEntityId);
        }
    }
}
