using SharedLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.Interface
{
    public interface IConnection
    {
        int Index { get; set; }
        bool Connected { get; set; }
        void CheckConnectionTimeOut();
        void Send(ByteBuffer msg, string name);
        void ReceiveData();
        void Disconnect();
        void SendPing();
    }
}
