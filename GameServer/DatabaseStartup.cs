
using Database;
using Database.Entities.Account;
using Database.Entities.Player;
using Database.Repositories.Account;
using Database.Repositories.Interface;
using Database.Repositories.Player;
using GameServer.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Util;

namespace GameServer.Database;

public class DatabaseStartup
{
    private static IServiceProvider _serviceProvider { get; set; }

    public DatabaseStartup(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static void Configure()
    {
        var serviceCollection = new ServiceCollection();

        // Configuração do DbContext e outros serviços
        var connectionString = @"Data Source=DatabaseSqlite.db";
        serviceCollection.AddDbContext<MeuDbContext>(options => options.UseSqlite(connectionString));

        // Configuração dos repositórios
        serviceCollection.AddScoped<IRepository<AccountEntity>, AccountRepository>();
        serviceCollection.AddScoped<IRepository<PlayerEntity>, PlayerRepository>();

        var provider = serviceCollection.BuildServiceProvider();

        Global.WriteLog(LogType.Database, $"Configurada com sucesso! {connectionString}", ConsoleColor.Green);

        _serviceProvider = provider;

        //Criação do banco de dados, caso não existir
        //using (var scope = provider.CreateScope())
        //{ var scp = scope.ServiceProvider.GetRequiredService<MeuDbContext>();
        //  scp.Database.Migrate(); }
    }

    public static async Task<List<PlayerEntity>> GetAccountCharacters(int accountID)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            // Resolvendo o serviço necessário dentro do escopo
            var accountService = scope.ServiceProvider.GetRequiredService<IRepository<AccountEntity>>();

            var accountRepo = (AccountRepository)accountService;

            var charsAccount = await accountRepo.GetCharactersByIdAccountAsync(accountID);

            Global.WriteLog(LogType.Database, charsAccount.Message, charsAccount.Color);

            if (charsAccount.Success)
            {
                foreach (var Char in charsAccount.Entity)
                {
                    
                    Global.WriteLog(LogType.Database, $"Character {Char.Name} {Char.Level}", ConsoleColor.Green);
                }

                return charsAccount.Entity;
            }
            else
            {
                return null;
            }
        }
    }
}