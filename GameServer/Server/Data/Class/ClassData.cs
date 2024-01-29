using Database.Entities.ValueObjects.Player;
using GameServer.Communication;
using SharedLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameServer.Server.Data.Class
{
    public class ClassData
    {
        public int MaxClasses { get; set; }
        public Dictionary<int, CharacterClass> Classes { get; set; }

    }
}
