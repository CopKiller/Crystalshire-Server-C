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

        public async Task<bool> NomeJogadorJaExisteAsync(string nome)
        {
            // Verificar se o nome já existe na tabela de jogadores
            return await _dbContext.PlayerEntities.AnyAsync(p => p.Name == nome);
        }

        public async Task<List<PlayerEntity>> AdicionarJogadorAsync(PlayerEntity jogador, int accountId)
        {
            try
            {
                // Verifique se o nome que o jogador escolheu já existe no banco de dados
                if (NomeJogadorJaExisteAsync(jogador.Name).Result)
                {
                    Console.WriteLine($"Jogador com nome {jogador.Name} já existe.");
                    return null;
                }   

                var playersAccount = await _dbContext.PlayerEntities
                .Where(p => p.AccountEntityId == accountId).OrderBy(p => p.Id).ToListAsync();

                // Verifique se os chars da conta existe no banco de dados
                if (playersAccount.Count <= 0)
                {
                    Console.WriteLine($"Jogador com ID {jogador.Id} não existe.");
                    return null;
                }

                // Procura se o slot que o jogador quer criar já existe um personagem
                // Caso não, cria o personagem no slot selecionado.
                foreach (var player in playersAccount)
                {
                    var index = playersAccount.IndexOf(player);

                    if (index == jogador.Id - 1)
                    {
                        if (string.IsNullOrWhiteSpace(player.Name))
                        {

                            playersAccount[index].Name = jogador.Name;
                            playersAccount[index].Sexo = jogador.Sexo;
                            playersAccount[index].ClassType = jogador.ClassType;
                            playersAccount[index].Sprite = jogador.Sprite;
                            // Salve as alterações no banco de dados
                            var result = await _dbContext.SaveChangesAsync();

                            if (result >= 0)
                            {
                                return playersAccount;
                            } else
                            {
                                Console.WriteLine($"Erro ao adicionar jogador: {player.Name}");
                                return null;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Jogador com nome {jogador.Name} já existe.");
                            return null;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar jogador: {ex.Message}");
                return null;
            }
        }

        public async Task<List<PlayerEntity>> ExcluirJogadorAsync(int charSlot, int accountId)
        {
            try
            {

                var playersAccount = await _dbContext.PlayerEntities
                .Where(p => p.AccountEntityId == accountId).OrderBy(p => p.Id).ToListAsync();

                var index = charSlot - 1;

               if (!string.IsNullOrEmpty(playersAccount[index].Name))
                {
                    playersAccount[index].Name = string.Empty;
                    playersAccount[index].Sexo = SexType.Male;
                    playersAccount[index].ClassType = ClassType.None;
                    playersAccount[index].Sprite = 1;
                    // Salve as alterações no banco de dados
                    var result = await _dbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        return playersAccount;
                    }
                    else
                    {
                        Console.WriteLine($"Erro ao excluir jogador: {playersAccount[index].Name}");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Tentativa de excluir um slot já vazio {charSlot}.");
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao excluir jogador: {ex.Message}");
                return null;
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
