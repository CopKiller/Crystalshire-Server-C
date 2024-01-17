using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities.ValueObjects
{
    public class Stat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;
        public int Strength { get; set; } = 3;
        public int Endurance { get; set; } = 3;
        public int Intelligence { get; set; } = 3;
        public int Agility { get; set; } = 3;
        public int WillPower { get; set; } = 3;
    }
}
