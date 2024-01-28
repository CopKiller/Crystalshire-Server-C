using Database.Entities.Configuration.Account;
using Database.Entities.Configuration.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities.Configuration
{
    public class Configuration : BaseEntity
    {
        public List<AccountNames> AccountNames = new List<AccountNames>();

        public List<PlayerNames> PlayerNames = new List<PlayerNames>();
    }
}
