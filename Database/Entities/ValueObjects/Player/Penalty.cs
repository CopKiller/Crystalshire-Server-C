using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities.ValueObjects.Player
{
    public class Penalty : BaseEntity
    {
        public bool isBanned { get; set; } = false;
        public bool isMuted { get; set; } = false;
    }
}
