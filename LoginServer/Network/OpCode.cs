
using LoginServer.Network.ClientPacket;
using LoginServer.Network.ServerPacket;

namespace LoginServer.Network {
    public sealed class OpCode {
        public static Dictionary<int, Type> RecvPacket = new Dictionary<int, Type>();
        public static Dictionary<Type, int> SendPacket = new Dictionary<Type, int>();

        public static void InitOpCode() {
            // Fluxo Receive
            RecvPacket.Add((int)Packet.CNewAccount, typeof(ServerPacket.CReceiveNewAccount));

            // Fluxo Send
            // Enviando um ping, pra saber o status da conexão!
            //SendPacket.Add(typeof(SpPing), (int)Packet.SSendPing);
        }
    }
}