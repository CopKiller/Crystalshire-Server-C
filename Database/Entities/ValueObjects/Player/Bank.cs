using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities.ValueObjects.Player
{
    public class Bank : BaseEntity
    {
        public int ItemId { get; set; } = 0;
        public int ItemAmount { get; set; } = 0;
        public byte Bounding { get; set; } = 0;
    }
}
