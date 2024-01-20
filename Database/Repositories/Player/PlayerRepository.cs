using Database.Entities.Player;

namespace Database.Repositories.Player
{
    public class PlayerRepository : RepositoryBase<PlayerEntity>
    {
        public PlayerRepository(MeuDbContext dbContext) : base(dbContext)
        {
        }

        public override object GetPrimaryKey(PlayerEntity entity) => entity.Id;

        // Adicione métodos específicos, se necessário
    }
}
