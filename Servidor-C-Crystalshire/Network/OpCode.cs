using GameServer.Network.ClientPacket;
using GameServer.Network.LoginPacket;
using GameServer.Network.ServerPacket;

namespace GameServer.Network;

public sealed class OpCode
{
    public static Dictionary<ClientPacketEnum, Type> RecvPacket = new();
    public static Dictionary<Type, ServerPacketEnum> SendPacket = new();

    public static void InitOpCode()
    {
        // Fluxo Receive
        RecvPacket.Add(ClientPacketEnum.CLogin, typeof(CReceiveLogin));

        RecvPacket.Add(ClientPacketEnum.UserData, typeof(CUserData));

        // Caso o jogador não esteja autenticado, recebe um direcionamento pro pacote abaixo.
        RecvPacket.Add(ClientPacketEnum.AuthLogin, typeof(CAuthenticateLogin));


        // Fluxo Send
        SendPacket.Add(typeof(SAlertMsg), ServerPacketEnum.SAlertMsg);
        
        SendPacket.Add(typeof(SPlayerLeft), ServerPacketEnum.SLeft);

        SendPacket.Add(typeof(SHighIndex), ServerPacketEnum.SHighIndex);

        SendPacket.Add(typeof(SPlayerChars), ServerPacketEnum.SPlayerChars);
    }
}