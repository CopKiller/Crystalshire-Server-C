using GameServer.Network.PacketList;
using GameServer.Network.PacketList.ClientPacket;
using GameServer.Network.PacketList.LoginPacket;
using GameServer.Network.PacketList.ServerPacket;

namespace GameServer.Network;

public static class OpCode
{
    public static Dictionary<ClientPacketEnum, Type> RecvPacket = new Dictionary<ClientPacketEnum, Type>();
    public static Dictionary<Type, ServerPacketEnum> SendPacket = new Dictionary<Type, ServerPacketEnum>();

    public static void InitOpCode()
    {
        // Fluxo Receive
        RecvPacket.Add(ClientPacketEnum.UserData, typeof(CUserData));

        // Caso o jogador não esteja autenticado, recebe um direcionamento pro pacote abaixo.
        RecvPacket.Add(ClientPacketEnum.CLogin, typeof(CLogin));

        RecvPacket.Add(ClientPacketEnum.CAddChar, typeof(CAddChar));

        RecvPacket.Add(ClientPacketEnum.CDelChar, typeof(CDelChar));


        // Fluxo Send
        SendPacket.Add(typeof(SAlertMsg), ServerPacketEnum.SAlertMsg);
        
        SendPacket.Add(typeof(SPlayerLeft), ServerPacketEnum.SLeft);

        SendPacket.Add(typeof(SHighIndex), ServerPacketEnum.SHighIndex);

        SendPacket.Add(typeof(SPlayerChars), ServerPacketEnum.SPlayerChars);

        SendPacket.Add(typeof(SClassesData), ServerPacketEnum.SClassesData);

        // Ping para verificar conexão.
        SendPacket.Add(typeof(SPing), ServerPacketEnum.SSendPing);
    }
}