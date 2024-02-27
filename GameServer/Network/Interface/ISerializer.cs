using SharedLibrary.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.Interface
{
    public interface ISerializer
    {
        public ByteBuffer Serialize<T>(T type, ByteBuffer buffer);
        object Deserialize(ByteBuffer buffer, Type type);
    }
}
