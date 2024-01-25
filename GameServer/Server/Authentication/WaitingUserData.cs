using GameServer.Communication;
using SharedLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Server.Authentication
{
    public sealed class WaitingUserData
    {
        public int ListIndex { get; set; }
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string UniqueKey { get; set; }

        private static int highIndex;
        private static Dictionary<int, WaitingUserData> users = new Dictionary<int, WaitingUserData>();

        public static void Add(WaitingUserData user)
        {
            var index = 0;

            if (users.Count < highIndex)
            {
                // Procura por um slot que não está sendo usado.
                for (var i = 1; i <= highIndex; i++)
                {
                    if (!users.ContainsKey(i))
                    {
                        index = i;
                        break;
                    }
                }
            }
            // Caso contrário, adiciona um novo slot.
            else
            {
                index = ++highIndex;
            }

            user.ListIndex = index;
            users.Add(index, user);
            Global.WriteLog(LogType.Player, $"Received Key {user.UniqueKey} Username {user.Username}", ConsoleColor.White);
        }

        public static WaitingUserData FindUser(string uniqueKey)
        {
            for (var n = 1; n <= highIndex; n++)
            {
                if (users.ContainsKey(n))
                {
                    if (string.Compare(users[n].UniqueKey, uniqueKey) == 0)
                    {
                        return users[n];
                    }
                }
            }

            return null;
        }

        public static void Remove(int index)
        {
            if (users.ContainsKey(index))
            {
                users.Remove(index);
            }
        }
    }
}
