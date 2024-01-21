using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities.ValueObjects.Player
{
    public class Skill : BaseEntity
    {
        public int SkillId { get; set; } = 0;
        public int SkillUses { get; set; } = 0;
    }
}
