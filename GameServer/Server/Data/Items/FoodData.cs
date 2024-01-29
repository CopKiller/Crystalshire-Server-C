using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Server.Data.Items
{
    public class FoodData
    {
        public int HPorSP { get; set; } = 0;
        public int FoodPerTick { get; set; } = 0;
        public int FoodTickCount { get; set; } = 0;
        public int FoodInterval { get; set; } = 0;
    }
}
