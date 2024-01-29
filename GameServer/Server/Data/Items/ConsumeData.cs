using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Server.Data.Items
{
    public class ConsumeData
    {
        public int AddHP { get; set; } = 0;
        public int AddMP { get; set; } = 0;
        public int AddEXP { get; set; } = 0;
        public int CastSpell { get; set; } = 0;
        public byte instaCast { get; set; } = 0;
    }
}
