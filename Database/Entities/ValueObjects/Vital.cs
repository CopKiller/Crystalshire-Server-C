using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities.ValueObjects
{
    public class Vital
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;
        public int CurHealth { get; set; } = 0;
        public int CurEnergy { get; set; } = 0;
        public int MaxHealth { get; set; } = 0;
        public int MaxEnergy { get; set; } = 0;
    }
}
