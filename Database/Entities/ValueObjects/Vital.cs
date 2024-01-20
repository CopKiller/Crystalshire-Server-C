using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities.ValueObjects
{
    public class Vital : BaseEntity
    {
        public int CurHealth { get; set; } = 0;
        public int CurEnergy { get; set; } = 0;
        public int MaxHealth { get; set; } = 0;
        public int MaxEnergy { get; set; } = 0;
    }
}
