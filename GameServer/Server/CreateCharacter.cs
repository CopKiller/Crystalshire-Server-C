using Database.Entities.Player;
using Database.Repositories.Interface;
using Database.Repositories.Player;
using GameServer.Communication;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Server
{
    public class Character
    {
        private IServiceProvider _serviceProvider;

        private readonly PlayerEntity player;
        public Character(PlayerEntity _player)
        {
            _serviceProvider = Global._serviceProvider;
            player = _player;
        }

        public void Create()
        {

        }

        public async Task<List<PlayerEntity>>? GetAccountCharacters(int accountID)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Resolvendo o serviço necessário dentro do escopo
                var playerService = scope.ServiceProvider.GetRequiredService<IRepository<PlayerEntity>>();

                var playerRepo = (PlayerRepository)playerService;

                var charsAccount = await playerRepo.GetPlayersByAccountIdAsync(accountID);

                if (charsAccount == null)
                {
                    Global.WriteLog(LogType.Database, $"Account {accountID} not found!", ConsoleColor.Red);
                    return null;
                }

                var counter = 0;
                foreach (var Char in charsAccount)
                {
                    counter++;
                    Global.WriteLog(LogType.Database, $"Character {counter} {Char.Name} {Char.Level}", ConsoleColor.Green);
                }

                return charsAccount;
            }
        }

        public async void SavePlayer(Player player)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                // Resolvendo o serviço necessário dentro do escopo
                var playerService = scope.ServiceProvider.GetRequiredService<IRepository<PlayerEntity>>();

                var playerRepo = (PlayerRepository)playerService;

                var playerEntity = await playerRepo.AtualizarJogadorAsync(player);

                if (!playerEntity)
                {
                    Global.WriteLog(LogType.Database, $"Player {player.Index} not found!", ConsoleColor.Red);
                    return;
                }
                else
                {
                    Global.WriteLog(LogType.Database, $"Player Index: {player.Index} Name: {player.Name} saved!", ConsoleColor.Green);
                }
            }
        }
    }
}
