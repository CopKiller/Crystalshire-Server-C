using Database.Entities.Player;
using Database.Repositories.Interface;
using Database.Repositories.Player;
using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Server.Authentication;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Util;

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
        public async void Save()
        {
            if (Authentication.Authentication.Players[Index].GameState != GameState.Game)
            {
                Global.WriteLog(LogType.Database, $"Player {this.Index} não está em jogo, por isso não foi salvo!", ConsoleColor.Red);
                return;
            }

            using (var scope = Global._serviceProvider.CreateScope())
            {
                // Resolvendo o serviço necessário dentro do escopo
                var playerService = scope.ServiceProvider.GetRequiredService<IRepository<PlayerEntity>>();

                var playerRepo = (PlayerRepository)playerService;

                var playerEntity = await playerRepo.AtualizarJogadorAsync(this);

                if (!playerEntity)
                {
                    Global.WriteLog(LogType.Database, $"Player {this.Index} not found!", ConsoleColor.Red);
                    return;
                }
                else
                {
                    Global.WriteLog(LogType.Database, $"Player Index: {this.Index} Name: {this.Name} saved!", ConsoleColor.Green);
                }
            }
        }

        //public MapInstance GetMap()
        //{
        //    return Global.Maps[MapId];
        //}
    }
}
