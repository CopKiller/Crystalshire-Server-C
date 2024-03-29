﻿using Database.Entities.Account;
using Database.Entities.Player;
using Database.Repositories.Interface;
using Database.Repositories.Player;
using GameServer.Communication;
using GameServer.Network.Interface;
using GameServer.Server.Authentication;
using GameServer.Server.PlayerData.Interface;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Util;

namespace GameServer.Server.PlayerData
{
    public sealed class PlayerData : AccountEntity, IPlayerData
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
        // Id da conta, sobrescrevendo o nome padrão "Id" da classe base, para melhor identificar
        public int AccountEntityId { get { return Id; } }

        // Slot do char que o jogador está logado.
        public int CharSlot { get; set; }

        public PlayerData(IConnection connection, WaitingUserData user)
        {
            Index = connection.Index;
            Connection = connection;
            Id = user.AccountId;
            UniqueKey = user.UniqueKey;
            GameState = GameState.Characters;
            Login = user.Username;
            CharSlot = 0;
        }

        public List<PlayerEntity> GivePlayerChars()
        {
            return Players;
        }
        public async void Save()
        {
            if (Authentication.Authentication.Players[Index].GameState != GameState.Game)
            {
                Global.WriteLog(LogType.Database, $"Player {Index} não está em jogo, por isso não foi salvo!", ConsoleColor.Red);
                return;
            }

            using (var scope = Global._serviceProvider.CreateScope())
            {
                // Resolvendo o serviço necessário dentro do escopo
                var playerService = scope.ServiceProvider.GetRequiredService<IRepository<PlayerEntity>>();

                var playerRepo = (PlayerRepository)playerService;

                var slotChar = Players.FindIndex(a => a.SlotId == CharSlot);

                var playerEntity = await playerRepo.AtualizarJogadorAsync(Players[slotChar]);

                if (!playerEntity)
                {
                    Global.WriteLog(LogType.Database, $"Player {Index} not found!", ConsoleColor.Red);
                    return;
                }
                else
                {
                    Global.WriteLog(LogType.Database, $"Player Index: {Index} Name: {Players[slotChar].Name} saved!", ConsoleColor.Green);
                }
            }
        }

        //public MapInstance GetMap()
        //{
        //    return Global.Maps[MapId];
        //}
    }
}
