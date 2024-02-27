using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities.ValueObjects.Player.Interface
{
    public interface IStat
    {
        int Strength { get; set; }
        int Endurance { get; set; }
        int Intelligence { get; set; }
        int Agility { get; set; }
        int WillPower { get; set; }
    }
}
