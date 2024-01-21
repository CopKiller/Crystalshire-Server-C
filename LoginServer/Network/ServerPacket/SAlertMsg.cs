using Database.Client;
using SharedLibrary.Network;

namespace LoginServer.Network.ServerPacket;

public sealed class SAlertMsg : SendPacket
{
    public SAlertMsg(ClientMessages messageType, ClientMenu MenuReset = 0, bool kick = true)
    {
        msg = new ByteBuffer();
        msg.Write(OpCode.SendPacket[GetType()]);
        msg.Write((int)messageType);
        msg.Write((int)MenuReset);
        msg.Write(kick ? 1 : 0);
    }
}