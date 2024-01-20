using Database.Entities.Account;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories.Account
{
    public class AccountRepository : RepositoryBase<AccountEntity>
    {
        private readonly MeuDbContext _dbContext;

        public override object GetPrimaryKey(AccountEntity entity) => entity.Id;

        public AccountRepository(MeuDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AdicionarContaAsync(AccountEntity account)
        {
            try
            {
                await _dbContext.AccountEntities.AddAsync(account);
                var result = await _dbContext.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao adicionar conta: {ex.Message}");
                return false;
            }
        }

        public async Task<int> AtualizarContaAsync()
        {
            try
            {
                var result = await _dbContext.SaveChangesAsync();

                return result; // Retorna o número de entidades atualizadas
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar conta: {ex.Message}");
                return 0;
            }
        }

        public async Task<AccountEntity> ObterContaPorLoginAsync(string login)
        {
            var conta = await _dbContext.AccountEntities
                .FirstOrDefaultAsync(e => e.Login == login);

            return conta;
        }

        public async Task<int> ExcluirContaPorLoginAsync(string login)
        {
            var accounts = await _dbContext.AccountEntities
                .Where(e => e.Login == login)
                .ToListAsync();

            if (accounts.Any())
            {
                _dbContext.AccountEntities.RemoveRange(accounts);
                await _dbContext.SaveChangesAsync();
                return accounts.Count;
            }

            return 0;
        }
    }
}
