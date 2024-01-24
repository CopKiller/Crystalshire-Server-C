using GameServer.Network;
using GameServer.Network.Interface;
using GameServer.Network.ServerPacket;

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
            highIndex.SendToAll();
        }

        public static void OnDisconnect(int index)
        {
            if (Players.ContainsKey(index))
            {

                SavePlayer(index);

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
            // Implementar a busca no banco de dados, pelo Id da conta.

            //var pData = from player in Players.Values
            //            where player.AccountEntityId == accountId
            //select player;

            //return pData.FirstOrDefault();

            return null;
        }

        public static Player FindByUsername(string username)
        {

            // Vai trazer o player do banco de dados, pelo username. Falta implementar a lógica
            // Talvez precise mudar a estrutura do banco, para salvar um login no playermodel do banco de dados.

            //var pData = from player in Players.Values
            //            where (string.CompareOrdinal(player.Username, username) == 0)
            //select player;

            //return pData.FirstOrDefault();

            return null;
        }

        public static Player FindByCharacter(string character)
        {
            // Implementar a busca no banco de dados, pelo nome do personagem.


            //var pData = from player in Players.Values
            //            where (string.CompareOrdinal(player.Character, character) == 0)
            //            select player;

            //return pData.FirstOrDefault();

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

            //var pData = Players[index];
            //var db = new CharacterDB();
            //var dbError = db.Open();

            //if (dbError.Number != 0)
            //{
            //    Global.WriteLog(LogType.System, $"Cannot connect to database", LogColor.Red);
            //    Global.WriteLog(LogType.System, $"Error Number: {dbError.Number}", LogColor.Red);
            //    Global.WriteLog(LogType.System, $"Error Message: {dbError.Message}", LogColor.Red);
            //}
            //else
            //{
            //    db.UpdateCharacter(pData);
            //    db.Close();
            //}
        }
    }
}
