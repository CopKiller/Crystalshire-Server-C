using GameServer.Network.Interface;
using GameServer.Server.Authentication;
using SharedLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.ClientPacket
{
    public sealed class CAuthenticateLogin : IRecvPacket
    {
        public void Process(byte[] buffer, IConnection connection)
        {
            var msg = new ByteBuffer(buffer);
            var username = msg.ReadString();
            var uniqueKey = msg.ReadString();

            if (username.Length > 0 && uniqueKey.Length > 0)
            {
                var authenticator = new WaitingUserAuthentication()
                {
                    Username = username,
                    UniqueKey = uniqueKey,
                    Connection = connection
                };

                authenticator.Authenticate();
            }
        }
    }
}
