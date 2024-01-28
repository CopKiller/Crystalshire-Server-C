using Database.Entities.Account;
using Database.Entities.Player;
using GameServer.Network.Interface;
using GameServer.Network.PacketList.ServerPacket;
using GameServer.Network.Tcp;
using GameServer.Server.PlayerData;
using GameServer.Server.PlayerData.Interface;

namespace GameServer.Server.Authentication
{
    public static class Authentication
    {
        public static Dictionary<int, IPlayerData> Players { get; set; }
        public static int HighIndex { get; set; }

        static Authentication()
        {
            Players = new Dictionary<int, IPlayerData>();
        }

        public static void Add(WaitingUserData user, IConnection connection)
        {
            var index = connection.Index;
            var pData = new PlayerData.PlayerData(connection, user);

            ((Connection)connection).OnDisconnect += OnDisconnect;
            ((Connection)connection).Authenticated = true;

            Players.Add(index, pData);

            var highIndex = new SHighIndex(HighIndex);
            Console.WriteLine($"HighIndex: {HighIndex}");
            highIndex.SendToAll();
        }

        public static IPlayerData FindByAccountId(int accountId)
        {
            return Players.Values.SingleOrDefault(player => player.AccountEntityId == accountId);
        }

        public static IPlayerData FindByUsername(string username)
        {
            var pData = from player in Players.Values
                        where string.Equals(player.Login, username, StringComparison.OrdinalIgnoreCase)
                        select player;

            return pData.FirstOrDefault();
        }

        public static IPlayerData FindByCharacter(string character)
        {
            var playerWithCharacter = Players.Values
                .FirstOrDefault(player => player.Players.Any(charInfo => string.Equals(charInfo.Name, character, StringComparison.OrdinalIgnoreCase)));

            return playerWithCharacter;
        }

        public static IPlayerData FindByIndex(int index)
        {
            if (Players.ContainsKey(index))
            {
                return Players[index];
            }

            return null;
        }

        public static PlayerEntity FindCharByIndex(int index)
        {
            if (Players.ContainsKey(index))
            {
                if (Players[index].CharSlot > 0 && Players[index].CharSlot <= AccountEntity.MaxChar)
                {
                    var charSlot = Players[index].Players.FindIndex(a => a.SlotId == Players[index].CharSlot);
                    if (charSlot != -1)
                    {
                        return Players[index].Players[charSlot];
                    }
                }
            }

            return null;
        }

        private static void SavePlayer(int index)
        {
            // Implementar a lógica para salvar um jogador no banco de dados.

            if (Players.ContainsKey(index))
            {
                Players[index].Save();
            }

        }

        public static void OnDisconnect(int index)
        {
            if (Players.ContainsKey(index))
            {
                if (Players[index].GameState == GameState.Game)
                {
                    if (Players[index].CharSlot > 0 && Players[index].CharSlot <= AccountEntity.MaxChar)
                    {
                        Players[index].Save();

                        var charSlot = Players[index].Players.FindIndex(a => a.SlotId == Players[index].CharSlot);
                        if (charSlot != -1)
                        {
                            if (Players[index].Players[charSlot].Position.MapNum > 0)
                            {
                                var msg = new SPlayerLeft(index);
                                msg.SendToMapBut(index, Players[index].Players[charSlot].Position.MapNum);
                            }
                        }
                    }
                }
            }

            Players.Remove(index);
        }

        public static void Clear()
        {
            Players.Clear();
        }

        public static void Quit(int index)
        {
            if (Players.ContainsKey(index))
            {
                Players[index].Connection.Disconnect();
            }
        }
    }
}
