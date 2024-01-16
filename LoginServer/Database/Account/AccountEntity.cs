using LoginServer.Database.Player;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer.Database.Account
{
    public class AccountEntity
    {
        private const byte Max_Char = 3;

        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set;} = DateTime.Now;
        public DateTime LastLoginDate { get; set;} = DateTime.Now;
        public PlayerEntity[] Player { get; set; } = new PlayerEntity[4];

        public AccountEntity()
        {
            // Adicionando elementos nos índices de 1 até o Max_Char
            for (int i = 1; i <= Max_Char; i++)
            {
                Player[i] = new PlayerEntity();
            }
        }
    }
}
