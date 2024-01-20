using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.ValueObjects
{
    public class Stat : BaseEntity
    {
        public int Strength { get; set; } = 3;
        public int Endurance { get; set; } = 3;
        public int Intelligence { get; set; } = 3;
        public int Agility { get; set; } = 3;
        public int WillPower { get; set; } = 3;
    }
}
