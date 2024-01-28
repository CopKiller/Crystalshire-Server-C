using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Database.Extension
{
    public static class EntityFrameworkExtension
    {
        public static async Task<int> DeleteAsync<TEntity>(
            this DbSet<TEntity> dbSet,
            Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            var entitiesToDelete = await dbSet
                .Where(predicate)
                .ToListAsync();

            dbSet.RemoveRange(entitiesToDelete);

            return entitiesToDelete.Count;
        }
    }

}
