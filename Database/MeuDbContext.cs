
using Database.Entities.Account;
using Database.Entities.Player;
using Database.Entities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SQLitePCL;

namespace Database;

public class MeuDbContext : DbContext
{
    public DbSet<AccountEntity> AccountEntities { get; set; }
    public DbSet<PlayerEntity> PlayerEntities { get; set; }

    public MeuDbContext()
    {
        Batteries.Init();
    }

    public MeuDbContext(DbContextOptions<MeuDbContext> options) : base(options)
    {
        Batteries.Init();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Configuração da exclusão em cascata para todas as relações
        foreach (var relationship in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }

        // Configuração de relações específicas
        modelBuilder.Entity<PlayerEntity>()
            .HasOne(p => p.Position)
            .WithOne()
            .HasForeignKey<Position>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlayerEntity>()
            .HasOne(p => p.Stat)
            .WithOne()
            .HasForeignKey<Stat>(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PlayerEntity>()
            .HasOne(p => p.Vital)
            .WithOne()
            .HasForeignKey<Vital>(v => v.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Adicione outras configurações de modelo, se necessário...
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(@"Data Source=DatabaseSqlite.db");
        }

        base.OnConfiguring(optionsBuilder);
    }
}