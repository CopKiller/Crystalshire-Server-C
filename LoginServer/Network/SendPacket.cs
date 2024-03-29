﻿using LoginServer.Network.Tcp;
using SharedLibrary.Network;
using SharedLibrary.Network.Interface;

namespace LoginServer.Network;

public abstract class SendPacket
{
    protected ByteBuffer msg;

    public SendPacket()
    {
        msg = new ByteBuffer();
    }

    public void Send(IConnection connection)
    {
        ((Connection)connection).Send(msg, GetType().Name);

        msg.Clear();
        msg = null;
    }
}