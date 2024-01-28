
using Database;
using Database.Configuration;
using Database.Entities.Account;
using Database.Entities.Player;
using Database.Repositories.Account;
using Database.Repositories.Interface;
using Database.Repositories.Player;
using LoginServer.Communication;
using LoginServer.Network.GamePacket;
using LoginServer.Network.ServerPacket;
using LoginServer.Network.Tcp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Client;
using SharedLibrary.Network.Interface;
using SharedLibrary.Util;

namespace LoginServer.Database;

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
        var connectionString = DatabaseDirectory.GetDatabaseDirectory();
        serviceCollection.AddDbContext<MeuDbContext>(options => options.UseSqlite(@connectionString));

        // Configuração dos repositórios
        serviceCollection.AddScoped<IRepository<AccountEntity>, AccountRepository>();
        serviceCollection.AddScoped<IRepository<PlayerEntity>, PlayerRepository>();

        var provider = serviceCollection.BuildServiceProvider();

        Global.WriteLog(LogType.Player, $"[DATABASE] Configurada com sucesso! {connectionString}", ConsoleColor.Yellow);

        _serviceProvider = provider;

        //Criação do banco de dados, caso não existir
        //using (var scope = provider.CreateScope())
        //{ var scp = scope.ServiceProvider.GetRequiredService<MeuDbContext>();
        //  scp.Database.Migrate(); }
    }

    public static async Task<OperationResult> AdicionarConta(string _Login, string _Password, string _Email)
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

            // Adicionando a conta e obtendo o resultado
            var resultadoAdicao = await accountRepo.AddPlayerAccountAsync(novaConta);

            Global.WriteLog(LogType.Database, resultadoAdicao.Message, resultadoAdicao.Color);

            // Retorna true se a adição foi bem-sucedida, caso contrário, retorna false
            return resultadoAdicao;
        }
    }

    public static async void Authenticate(IConnection connection,string _Login, string _Password)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            // Resolvendo o serviço necessário dentro do escopo
            var accountService = scope.ServiceProvider.GetRequiredService<IRepository<AccountEntity>>();

            var accountRepo = (AccountRepository)accountService;

            // Recupera a conta recém-adicionada por login
            var contaRecuperada = await accountRepo.AuthenticateAccount(_Login, _Password);

            Global.WriteLog(LogType.Database, contaRecuperada.Message, contaRecuperada.Color);
            
            if (contaRecuperada.Success)
            {
                var msgToGameServer = new SSendUserData()
                {
                    AccountId = contaRecuperada.Entity.Id,
                    Username = contaRecuperada.Entity.Login,
                    UniqueKey = ((Connection)connection).UniqueKey
                };
                msgToGameServer.Send();

                var msgToClient = new SLoginToken(_Login, ((Connection)connection).UniqueKey);
                msgToClient.Send(connection);
            } else
            {
                new SAlertMsg(ClientMessages.WrongPass).Send(connection);
            }

            //Desconecta o player
            connection.Disconnect();
        }
    }

    public static async Task ExcluirConta(string _Login)
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
                Global.WriteLog(LogType.Player, $"[DATABASE] Erro ao excluir a conta.", ConsoleColor.Red);
            }
        }
    }

    public static async Task AtualizarContas()
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