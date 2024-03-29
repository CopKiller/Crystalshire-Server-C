﻿using LoginServer.Network.ClientPacket;
using LoginServer.Network.GamePacket;
using LoginServer.Network.PacketList;
using LoginServer.Network.PacketList.GamePacket;
using LoginServer.Network.ServerPacket;

namespace LoginServer.Network;

public sealed class OpCode
{
    public static Dictionary<int, Type> RecvPacket = new();
    public static Dictionary<Type, int> SendPacket = new();

    public static void InitOpCode()
    {
        // Fluxo Receive
        RecvPacket.Add((int)ClientPacketEnum.CNewAccount, typeof(CReceiveNewAccount));
        RecvPacket.Add((int)ClientPacketEnum.CLogin, typeof(CReceiveLogin));

        // Fluxo Send
        SendPacket.Add(typeof(SAlertMsg), (int)ServerPacketEnum.SAlertMsg); // Envia pro cliente uma mensagem de alerta
        SendPacket.Add(typeof(SLoginToken), (int)ServerPacketEnum.LoginToken); // Envia pro cliente o token de login
        SendPacket.Add(typeof(SSendUserData), (int)ServerPacketEnum.UserData); // Envia pro GameServer os dados do usuário
        
        
        SendPacket.Add(typeof(SPing), (int)ServerPacketEnum.SSendPing); // Envia pro GameServer os dados do usuário
    }
}