using Database.Entities.ValueObjects.Player.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.ValueObjects.Player
{
    public class Stat : BaseEntity, IStat
    {
        public int Strength { get; set; } = 3;
        public int Endurance { get; set; } = 3;
        public int Intelligence { get; set; } = 3;
        public int Agility { get; set; } = 3;
        public int WillPower { get; set; } = 3;
    }
}
