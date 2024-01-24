using Database.Entities.Account;
using Database.Entities.Player;
using Database.Migrations;
using Database.Repositories.ValidateData;
using Microsoft.EntityFrameworkCore;

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
        // Adicione métodos específicos, se necessário
    }
}
