
using Database;
using Database.Entities.Account;
using Database.Entities.Player;
using Database.Repositories.Account;
using Database.Repositories.Interface;
using Database.Repositories.Player;
using LoginServer.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Util;

namespace LoginServer.Database;

public class DatabaseStartup
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseStartup(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static IServiceProvider Configure()
    {
        var serviceCollection = new ServiceCollection();

        // Configuração do DbContext e outros serviços
        var connectionString = @"Data Source=DatabaseSqlite.db";
        serviceCollection.AddDbContext<MeuDbContext>(options => options.UseSqlite(connectionString));

        Console.WriteLine(connectionString);

        // Configuração dos repositórios
        serviceCollection.AddScoped<IRepository<AccountEntity>, AccountRepository>();
        serviceCollection.AddScoped<IRepository<PlayerEntity>, PlayerRepository>();

        var provider = serviceCollection.BuildServiceProvider();

        Global.WriteLog(LogType.Player, $"[DATABASE] Configurada com sucesso! {connectionString}", ConsoleColor.Yellow);

        using (var scope = provider.CreateScope())
        {
            // Resolvendo o serviço necessário dentro do escopo
            var migration = 0;

            if (migration > 0)
            {
                var scp = scope.ServiceProvider.GetRequiredService<MeuDbContext>();
                scp.Database.Migrate();

                migration = 0;
            }

            Global.WriteLog(LogType.System, "[DATABASE] Banco de dados migrado com sucesso!", ConsoleColor.Yellow);
        }

        return provider;
    }

    public async Task AdicionarConta(string _Login, string _Password, string _Email)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            // Resolvendo o serviço necessário dentro do escopo
            var accountService = scope.ServiceProvider.GetRequiredService<IRepository<AccountEntity>>();

            var accountRepo = (AccountRepository)accountService;

            // Adicionando uma nova conta
            var novaConta = new AccountEntity
            {
                Login = _Login,
                Password = _Password,
                Email = _Email
            };

            var adicionouComSucesso = await accountRepo.AdicionarContaAsync(novaConta);

            if (adicionouComSucesso)
            {
                Global.WriteLog(LogType.Player, $"[DATABASE] Conta adicionada com sucesso!", ConsoleColor.Yellow);
            }
            else
            {
                Console.WriteLine("Erro ao adicionar a conta.");
                Global.WriteLog(LogType.Player, $"[DATABASE] Erro ao adicionar a conta.", ConsoleColor.Red);
            }
        }
    }

    public async Task RecuperarConta(string _Login)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            // Resolvendo o serviço necessário dentro do escopo
            var accountService = scope.ServiceProvider.GetRequiredService<IRepository<AccountEntity>>();

            var accountRepo = (AccountRepository)accountService;

            // Recupera a conta recém-adicionada por login
            var contaRecuperada = await accountRepo.ObterContaPorLoginAsync(_Login);

            if (contaRecuperada != null)
            {
                Global.WriteLog(LogType.Player, $"[DATABASE] Conta recuperada: {contaRecuperada.Login}, {contaRecuperada.Email}", ConsoleColor.Yellow);
            }
            else
            {
                Global.WriteLog(LogType.Player, $"[DATABASE] Erro ao recuperar a conta.", ConsoleColor.Red);
            }
        }
    }

    public async Task ExcluirConta(string _Login)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            // Resolvendo o serviço necessário dentro do escopo
            var accountService = scope.ServiceProvider.GetRequiredService<IRepository<AccountEntity>>();

            var accountRepo = (AccountRepository)accountService;

            // Recupera a conta recém-adicionada por login
            var contaExcluida = await accountRepo.ExcluirContaPorLoginAsync(_Login);

            if (contaExcluida != 0)
            {
                Global.WriteLog(LogType.Player, $"[DATABASE] Contas excluídas: {contaExcluida}", ConsoleColor.Yellow);
            }
            else
            {
                Global.WriteLog(LogType.Player, $"[DATABASE] Erro ao recuperar a conta.", ConsoleColor.Red);
            }
        }
    }

    public async Task AtualizarContas()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            // Resolvendo o serviço necessário dentro do escopo
            var accountService = scope.ServiceProvider.GetRequiredService<IRepository<AccountEntity>>();

            var accountRepo = (AccountRepository)accountService;

            // Atualiza as contas
            var contasAtualizadas = await accountRepo.AtualizarContaAsync();

            if (contasAtualizadas > 0)
            {
                Global.WriteLog(LogType.Player, $"[DATABASE] Contas Atualizadas: {contasAtualizadas}", ConsoleColor.Yellow);
            }
            else
            {
                Global.WriteLog(LogType.Player, $"[DATABASE] Nenhuma conta foi atualizada", ConsoleColor.Red);
            }
        }
    }
}