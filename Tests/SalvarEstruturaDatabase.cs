using Database;
using Database.Entities.Account;
using Database.Entities.Player;
using Database.Repositories.Account;
using Database.Repositories.Interface;
using Database.Repositories.Player;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Tests
{
    //[TestClass]
    //public class SalvarEstruturaDatabase
    //{
    //    public SalvarEstruturaDatabase()
    //    {
    //        var services = new ServiceCollection();
    //        services.AddDbContext<MeuDbContext>(options => options.UseSqlite("Server=(localdb)\\mssqllocaldb;Database=MeuDbContext;Trusted_Connection=True;MultipleActiveResultSets=true"));
    //        services.AddScoped<IPlayerRepository, PlayerRepository>();
    //        services.AddScoped<IAccountRepository, AccountRepository>();
    //        var serviceProvider = services.BuildServiceProvider();
    //        var dbContext = serviceProvider.GetService<MeuDbContext>();
    //        dbContext.Database.EnsureCreated();
    //    }
    //}   
}
