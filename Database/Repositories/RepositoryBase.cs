using Database.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Database.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;

        protected RepositoryBase(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public abstract object GetPrimaryKey(T entity);

        public async Task<bool> AddAsync(T entity)
        {
            var entry = _dbContext.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                var primaryKey = GetPrimaryKey(entity);

                if (await _dbContext.Set<T>().FindAsync(primaryKey) == null)
                {
                    entry.State = EntityState.Modified;
                    await _dbContext.Set<T>().AddAsync(entity);
                    return true; // Retorna true se a adição for bem-sucedida
                }
            }

            return false; // Retorna false se a adição não for realizada
        }

        public async Task<bool> UpdateAsync()
        {
            try
            {
                var result = await _dbContext.SaveChangesAsync();

                // Se o número de alterações no banco de dados for maior que zero, consideramos a operação bem-sucedida
                return result > 0;
            }
            catch (Exception ex)
            {
                // Manipule exceções, registre logs, etc., conforme necessário
                Console.WriteLine($"Erro ao atualizar a entidade: {ex.Message}");
                return false;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
