using LoginServer.Network.ClientPacket;

namespace LoginServer.Network;

public sealed class OpCode
{
    public static Dictionary<int, Type> RecvPacket = new();
    public static Dictionary<Type, int> SendPacket = new();

    public static void InitOpCode()
    {
        // Fluxo Receive
        RecvPacket.Add((int)Packet.CNewAccount, typeof(CReceiveNewAccount));

        // Fluxo Send
        // Enviando um ping, pra saber o status da conexão!
        //SendPacket.Add(typeof(SpPing), (int)Packet.SSendPing);
    }
}