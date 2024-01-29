using GameServer.Server.Data;
using GameServer.Server.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.PacketList.ServerPacket
{
    public sealed class SUpdateItemTo : SendPacket
    {
        List<ItemData> itemsList = Configuration.Items;
        ItemData[] itemsArray;
        public SUpdateItemTo()
        {
            itemsArray = itemsList.ToArray();
        }

        public SUpdateItemTo(int itemId)
        {
            itemsArray = itemsList.ToArray();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(memoryStream))
                {
                    IntPtr itemPtr = Marshal.UnsafeAddrOfPinnedArrayElement(itemsArray, itemId - 1);
                    byte[] itemData = new byte[Marshal.SizeOf(typeof(ItemData))];

                    Marshal.Copy(itemPtr, itemData, 0, itemData.Length);

                    writer.Write((int)OpCode.SendPacket[GetType()]);
                    writer.Write(itemId);
                    writer.Write(itemData);
                }
            }
        }

        public void ItemAll()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(memoryStream))
                {
                    for (int i = 0; i < itemsList.Count; i++)
                    {
                        byte[] itemData = new byte[Marshal.SizeOf(typeof(ItemData))];
                        IntPtr itemPtr = Marshal.UnsafeAddrOfPinnedArrayElement(itemsArray, i);
                        Marshal.Copy(itemPtr, itemData, 0, itemData.Length);

                        writer.Write((int)OpCode.SendPacket[GetType()]);
                        writer.Write(i + 1); // Use a propriedade Id ou outra propriedade única do item como identificador
                        writer.Write(itemData);
                    }
                }
                msg.Write(memoryStream.ToArray());
            }
        }
    }
}
