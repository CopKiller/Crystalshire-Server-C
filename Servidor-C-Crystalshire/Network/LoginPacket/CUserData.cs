using SharedLibrary.Network.Interface;
using SharedLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using GameServer.Server.Authentication;


namespace GameServer.Network.LoginPacket
{
    public sealed class CUserData : IRecvPacket
    {
        public void Process(byte[] buffer, IConnection connection)
        {
            var msg = new ByteBuffer(buffer);

            WaitingUserData.Add(
                new WaitingUserData()
                {
                    AccountId = msg.ReadInt32(),
                    Username = msg.ReadString(),
                    UniqueKey = msg.ReadString(),
                }
            );
        }
    }
}
