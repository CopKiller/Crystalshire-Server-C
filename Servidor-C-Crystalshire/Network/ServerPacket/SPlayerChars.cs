


using Database.Entities.Account;
using Database.Entities.Player;
using GameServer.Server;
using Microsoft.Extensions.Configuration;
using SharedLibrary.Client;
using SharedLibrary.Network;

namespace GameServer.Network.ServerPacket;

public sealed class SPlayerChars : SendPacket
{
    public SPlayerChars(List<PlayerEntity> player)
    {
        msg.Write((int)OpCode.SendPacket[GetType()]);

        foreach (var p in player)
        {
            msg.Write(p.Name);
            msg.Write(p.Sprite);
            msg.Write((int)p.AccessType);
            msg.Write((int)p.ClassType);
        }
    }
}