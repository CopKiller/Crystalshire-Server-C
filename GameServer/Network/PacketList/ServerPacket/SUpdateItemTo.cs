using Database.Entities.ValueObjects.Player;
using GameServer.Server.Data;
using GameServer.Server.Data.Items;
using SharedLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.PacketList.ServerPacket
{
    public sealed class SUpdateItemTo : SendPacket
    {
        List<ItemData> itemsList = Configuration.Items;
        public SUpdateItemTo(int itemId)
        {
            msg.Write((int)OpCode.SendPacket[GetType()]);

            Configuration.Items[itemId - 1].Name = "Teste"; // Tamanho 5
            Configuration.Items[itemId - 1].Description = "TesteDesc"; // Tamanho 9
            Configuration.Items[itemId - 1].Sound = "testSouund"; // Tamanho 10
            Configuration.Items[itemId - 1].Pic = 157; // Tamanho 10
            Configuration.Items[itemId - 1].Type = (ItemType)2; // Tamanho 10
            Configuration.Items[itemId - 1].StatReq = new Stat { Strength = 10, Endurance = 10, Intelligence = 10, Agility = 10, WillPower = 10 };
            Configuration.Items[itemId - 1].AddStat = new Stat { Strength = 20, Endurance = 20, Intelligence = 20, Agility = 20, WillPower = 20 };
            Configuration.Items[itemId - 1].Proficiency = 25;

            msg.Write(itemId);
            msg.Write(new ClassSerializer().Serialize(itemsList[itemId - 1]));
        }
    }
}
