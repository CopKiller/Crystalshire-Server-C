using Database.Entities.ValueObjects.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameServer.Server.Data.Class
{
    public class CharacterClass
    {
        public string Name { get; set; }
        public Stat Stats { get; set; }
        public int[] MaleSprite { get; set; }
        public int[] FemaleSprite { get; set; }
        public ItemClassData[] StartItem { get; set; }
        public int[] StartSpellId { get; set; }

        public Vital GetClassVitals()
        {
            var vital = new Vital();

            vital.MaxHealth = 100 + Stats.Endurance * 5 + 2; ;
            vital.MaxEnergy = 30 + Stats.Intelligence * 10 + 2;

            return vital;
        }
        public Stat GetClassStats()
        {
            return Stats;
        }
    }

    public class ItemClassData
    {
        public int ItemId { get; set; }
        public int ItemCount { get; set; }
    }

}
