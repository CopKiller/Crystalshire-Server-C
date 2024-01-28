using Database.Entities.Player;
using Database.Repositories.Interface;
using Database.Repositories.Player;
using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Network.PacketList.ClientPacket;
using GameServer.Network.PacketList.ServerPacket;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Client;
using SharedLibrary.Util;
using System.Text.RegularExpressions;

namespace GameServer.Server
{
    public class Character
    {
        private IServiceProvider _serviceProvider;

        private readonly PlayerEntity player;
        public Character()
        {
            _serviceProvider = Global._serviceProvider;
        }

        public Character(PlayerEntity player)
        {
            this.player = player;
            _serviceProvider = Global._serviceProvider;
        }

        public async Task<List<PlayerEntity>> Create(int accountId)
        {

            using (var scope = _serviceProvider.CreateScope())
            {
                // Resolvendo o serviço necessário dentro do escopo
                var playerService = scope.ServiceProvider.GetRequiredService<IRepository<PlayerEntity>>();

                var playerRepo = (PlayerRepository)playerService;

                var result = await playerRepo.AdicionarJogadorAsync(player, accountId);

                return result;
            }
        }

        public async void Exclude(int index, int charSlot, int accountId)
        {
            var player = Authentication.Authentication.Players[index];

            if (player.Connection == null || !player.Connected)
            {
                return;
            }
            if (index <= 0 || index > Authentication.Authentication.HighIndex)
            {
                new SAlertMsg(ClientMessages.Connection, ClientMenu.MenuMain).Send(player.Connection);
            }
            if (player.GameState != GameState.Characters)
            {
                Global.WriteLog(LogType.Database, $"Player {index} não está no menu de personagens, por isso não foi excluído!", ConsoleColor.Red);
                new SAlertMsg(ClientMessages.Connection, ClientMenu.MenuMain).Send(player.Connection);
                return;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                // Resolvendo o serviço necessário dentro do escopo
                var playerService = scope.ServiceProvider.GetRequiredService<IRepository<PlayerEntity>>();

                var playerRepo = (PlayerRepository)playerService;

                var result = await playerRepo.ExcluirJogadorAsync(charSlot, accountId);

                if (result != null)
                {
                    Global.WriteLog(LogType.Database, $"Character {charSlot} deleted!", ConsoleColor.Green);
                    new SAlertMsg(ClientMessages.DelChar, ClientMenu.MenuChars).Send(player.Connection);
                }
                else
                {
                    Global.WriteLog(LogType.Database, $"Character {charSlot} not deleted!", ConsoleColor.Red);
                    new SAlertMsg(ClientMessages.MySql, ClientMenu.MenuChars).Send(player.Connection);
                }
            }
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
    }
}
