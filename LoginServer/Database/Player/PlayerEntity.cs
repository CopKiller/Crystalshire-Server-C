using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Database.Player
{
    public class PlayerEntity
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public SexType Sexo { get; set; } = SexType.None;
        public ClassType ClassType { get; set; } = ClassType.None;
        public AccessType AccessType { get; set; } = AccessType.Player;
        public EntidadeType Entidade { get; set; } = EntidadeType.Normal;
        public int Sprite { get; set; } = 1;
        public int Level { get; set; } = 1;
        public int Exp { get; set; } = 1;


    }
}
