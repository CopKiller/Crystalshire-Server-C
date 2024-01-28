using GameServer.Network.Interface;
using GameServer.Network.PacketList.ServerPacket;
using GameServer.Network.Tcp;

namespace GameServer.Server.Authentication
{
    public static class Authentication
    {
        public static Dictionary<int, Player> Players { get; set; }
        public static int HighIndex { get; set; }

        static Authentication()
        {
            Players = new Dictionary<int, Player>();
        }

        public static void Add(WaitingUserData user, IConnection connection)
        {
            var index = connection.Index;
            var pData = new Player(connection, user);

            ((Connection)connection).OnDisconnect += OnDisconnect;
            ((Connection)connection).Authenticated = true;

            Players.Add(index, pData);

            var highIndex = new SHighIndex(HighIndex);
            Console.WriteLine($"HighIndex: {HighIndex}");
            highIndex.SendToAll();
        }

        public static void OnDisconnect(int index)
        {
            if (Players.ContainsKey(index))
            {

                Players[index].Save();

                var msg = new SPlayerLeft(index);
                msg.SendToMapBut(index, Players[index].Position.MapNum);
            }

            Players.Remove(index);
        }

        public static void Clear()
        {
            Players.Clear();
        }

        public static Player FindByAccountId(int accountId)
        {
            var pData = from player in Players.Values
                        where player.AccountEntityId == accountId
            select player;

            return pData.FirstOrDefault();
        }

        public static Player FindByUsername(string username)
        {

            // Procura pelo nome do usuário na lista de jogadores.
            var pData = from player in Players.Values
                        where (string.CompareOrdinal(player.Login, username) == 0)
            select player;

            return pData.FirstOrDefault();
        }

        public static Player FindByCharacter(string character)
        {
            var pData = from player in Players.Values
                        where (string.CompareOrdinal(player.Name, character) == 0)
                        select player;

            return pData.FirstOrDefault();
        }

        public static Player FindByIndex(int index)
        {
            if (Players.ContainsKey(index))
            {
                return Players[index];
            }

            return null;
        }

        public static void Quit(int index)
        {
            if (Players.ContainsKey(index))
            {
                Players[index].Connection.Disconnect();
            }
        }

        private static void SavePlayer(int index)
        {
            // Implementar a lógica para salvar um jogador no banco de dados.

            if (Players.ContainsKey(index))
            {
                Players[index].Save();
            }

        }
    }
}
