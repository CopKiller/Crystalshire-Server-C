using Database.Entities.Account;
using Database.Entities.Player;
using Database.Extension;
using Database.Migrations;
using Database.Repositories.ValidateData;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Client;
using System.Numerics;

namespace Database.Repositories.Player
{
    public class PlayerRepository : RepositoryBase<PlayerEntity>
    {
        private readonly MeuDbContext _dbContext;

        public override object GetPrimaryKey(PlayerEntity entity) => entity.Id;

        public PlayerRepository(MeuDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PlayerEntity>> GetPlayersByAccountIdAsync(int accountId)
        {
            return await _dbContext.PlayerEntities
                .Where(p => p.AccountEntityId == accountId)
                .ToListAsync();
        }

        //public async Task<List<PlayerEntity>> GetPlayerByAccountIdAsync(int accountId, int charSlot)
        //{
        //    return await _dbContext.PlayerEntities
        //        .Where(p => p.AccountEntityId == accountId && p.SlotId == charSlot)
        //        .ToListAsync();
        //}

        public async Task<bool> NomeJogadorJaExisteAsync(string nome)
        {
            // Verificar se o nome já existe na tabela de jogadores
            return await _dbContext.PlayerEntities.AnyAsync(p => p.Name.ToLower() == nome.ToLower());
        }

        public async Task<bool> AdicionarJogadorAsync(PlayerEntity jogador)
        {
            try
            {
                // Verifique se o nome que o jogador escolheu já existe no banco de dados
                if (NomeJogadorJaExisteAsync(jogador.Name).Result)
                {
                    Console.WriteLine($"Jogador com nome {jogador.Name} já existe.");
                    return false;
                }

                // Validar a quantidade de chars da conta, para não ultrapassar o limite
                int quantidadeJogadores = _dbContext.PlayerEntities
                .Count(p => p.AccountEntityId == jogador.AccountEntityId);
                if (quantidadeJogadores >= AccountEntity.MaxChar)
                {
                    Console.WriteLine($"A conta {jogador.AccountEntityId} já possui 3 personagens.");
                    return false;
                }

                // Verificar se o jogador não está tentando criar um char onde já tem um.
                var playersAccount = await _dbContext.PlayerEntities
                .Where(p => p.AccountEntityId == jogador.AccountEntityId).AnyAsync(a => a.SlotId == jogador.SlotId);
                if (playersAccount)
                {
                    Console.WriteLine($"O Slot {jogador.SlotId} já possui um personagem.");
                    return false;
                }

                // Adicionar o jogador no banco de dados
                await _dbContext.PlayerEntities.AddAsync(jogador);
                // Salve as alterações no banco de dados
                var result = await _dbContext.SaveChangesAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar jogador: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ExcluirJogadorAsync(int charSlot, int accountId)
        {
            try
            {
                var result = await _dbContext.PlayerEntities
                .DeleteAsync(p => p.AccountEntityId == accountId && p.SlotId == charSlot) > 0;

                if (result)
                {
                    await _dbContext.SaveChangesAsync();
                    Console.WriteLine($"Jogador com Slot {charSlot} e AccountId {accountId} foi excluído.");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Jogador com Slot {charSlot} e AccountId {accountId} não foi excluído.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir jogador: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AtualizarJogadorAsync(PlayerEntity jogador)
        {
            try
            {
                // Verifique se o jogador existe no banco de dados
                var jogadorExistente = await _dbContext.PlayerEntities.FindAsync(jogador.Id);
                if (jogadorExistente == null)
                {
                    Console.WriteLine($"Jogador com ID {jogador.Id} não encontrado.");
                    return false;
                }

                jogadorExistente = jogador;
                // Atualize outras propriedades conforme necessário

                // Salve as alterações no banco de dados
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar jogador: {ex.Message}");
                return false;
            }
        }

        // Adicione métodos específicos, se necessário
    }
}
