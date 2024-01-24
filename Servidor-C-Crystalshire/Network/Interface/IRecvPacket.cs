﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Network.Interface
{
    public interface IRecvPacket
    {
        void Process(byte[] buffer, IConnection connection);
    }
}
