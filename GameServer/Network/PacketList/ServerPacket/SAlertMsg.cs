using SharedLibrary.Client;
using SharedLibrary.Network;

namespace GameServer.Network.PacketList.ServerPacket;

public sealed class SAlertMsg : SendPacket
{
    public SAlertMsg(ClientMessages messageType, ClientMenu MenuReset = 0, bool kick = true, string customMsg = "")
    {
        //msg = new ByteBuffer();
        msg.Write((int)OpCode.SendPacket[GetType()]);
        msg.Write((int)messageType);
        msg.Write((int)MenuReset);
        msg.Write(kick ? 1 : 0);

        msg.Write(customMsg);
    }
}