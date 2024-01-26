using Database.Entities.Player;
using GameServer.Network.Interface;
using GameServer.Server.Authentication;

namespace GameServer.Server
{
    public sealed class Player: PlayerEntity
    {
        public IConnection Connection { get; set; }

        public bool Connected
        {
            get
            {
                if (Connection != null)
                {
                    return Connection.Connected;
                }

                return false;
            }
        }

        // Chave única de identificação no sistema.
        public string UniqueKey { get; set; }
        // Id de indice de conexão.
        public int Index { get; set; }
        // Tempo de conexão em segundos.
        public int ConnectedTime { get; set; }
        // Estado atual do usuário no sistema.
        public GameState GameState { get; set; }
        // Id da conta, para futuro acesso ao banco de dados.
        public int AccountEntityId { get; set; }

        public string Login { get; set; }

        public Player(IConnection connection, WaitingUserData user)
        {
            Index = connection.Index;
            Connection = connection;
            AccountEntityId = user.AccountId;
            UniqueKey = user.UniqueKey;
            GameState = GameState.Characters;
            Login = user.Username;
        }

        //public MapInstance GetMap()
        //{
        //    return Global.Maps[MapId];
        //}
    }
}
