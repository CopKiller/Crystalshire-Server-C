


using Database.Entities.Account;
using Database.Entities.Player;
using GameServer.Server;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Client;
using SharedLibrary.Network;

namespace GameServer.Network.PacketList.ServerPacket;

public sealed class SPlayerChars : SendPacket
{
    public SPlayerChars(List<PlayerEntity> player)
    {
        msg.Write((int)OpCode.SendPacket[GetType()]);

        msg.Write(player.Count);

        foreach (var p in player)
        {
            msg.Write(p.SlotId);
            msg.Write(p.Name);
            msg.Write(p.Sprite);
            msg.Write((int)p.AccessType);
            msg.Write((int)p.ClassType);
        }
    }
}