using Database.Entities.Account;
using Database.Entities.Player;
using Database.Migrations;
using Database.Repositories.ValidateData;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Client;

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

        public async Task<bool> AdicionarJogadorAsync(PlayerEntity jogador)
        {
            try
            {
                // Adicione lógica de validação, se necessário
                await _dbContext.PlayerEntities.AddAsync(jogador);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar jogador: {ex.Message}");
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

                // Atualize as propriedades do jogador existente
                jogadorExistente.Name = jogador.Name;
                jogadorExistente.Level = jogador.Level;
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
