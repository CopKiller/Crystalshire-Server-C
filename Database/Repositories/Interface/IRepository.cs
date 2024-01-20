namespace Database.Repositories.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<bool> UpdateAsync();
        Task DeleteAsync(T entity);

        object GetPrimaryKey(T entity);
    }
}
