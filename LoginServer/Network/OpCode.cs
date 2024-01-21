using LoginServer.Network.ClientPacket;
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

        // Fluxo Send
        SendPacket.Add(typeof(SAlertMsg), (int)ServerPacketEnum.SAlertMsg);
    }
}