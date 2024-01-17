
using Database.Entities.Account;
using Database.Entities.Player;
using Database.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Database;

public class SeuDbContext : DbContext
{
    public DbSet<AccountEntity> AccountEntities { get; set; }
    public DbSet<PlayerEntity> PlayerEntities { get; set; }
    //public DbSet<Position> Positions { get; set; }
    //public DbSet<Stat> Stat { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Adicione outras configurações de modelo, se necessário...

        base.OnModelCreating(modelBuilder);

        // Obtém todas as entidades no modelo
        //var entityTypes = modelBuilder.Model.GetEntityTypes();

        //// Aplica HasNoKey para cada entidade
        //foreach (var entityType in entityTypes)
        //{
        //    modelBuilder.Entity(entityType.ClrType).HasNoKey();
        //}
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DatabaseSqlite.db");
        Batteries.Init();
    }
}