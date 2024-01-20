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
    [TestClass]
    public class MeuDbContextTest
    {
        private IServiceProvider _serviceProvider;

        [TestInitialize]
        public void Setup()
        {
            var serviceCollection = new ServiceCollection();

            // Utiliza o SQLite em disco para os testes
            var connectionString = "Data Source=TestDatabase.db";
            serviceCollection.AddDbContext<MeuDbContext>(options =>
                options.UseSqlite(connectionString));

            serviceCollection.AddScoped<IRepository<AccountEntity>, AccountRepository>();
            serviceCollection.AddScoped<IRepository<PlayerEntity>, PlayerRepository>();

            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Garante que o banco de dados está criado e atualizado
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MeuDbContext>();
                dbContext.Database.Migrate();
            }
        }

        [TestMethod]
        public async Task AdicionarConta_DeveRetornarTrue()
        {
            // Arrange
            using (var scope = _serviceProvider.CreateScope())
            {
                var accountRepository = scope.ServiceProvider.GetRequiredService<IRepository<AccountEntity>>();

                // Act
                var account = new AccountEntity
                {
                    Login = "LoginAqui",
                    Password = "SenhaAqui",
                    Email = "EmailAqui"
                };

                var result = await accountRepository.AddAsync(account);

                // Assert
                Assert.IsTrue(result, "Falha ao adicionar a conta no banco de dados.");

                // Atualiza a conta
                var result1 = await accountRepository.UpdateAsync();

                // Assert
                Assert.IsTrue(result1, "Falha ao atualizar a conta no banco de dados.");

                // Use o ID da conta armazenado para obter a conta
                var retrievedAccount = await accountRepository.GetByIdAsync(account.Id);

                // Assert
                Assert.IsNotNull(retrievedAccount, "Falha ao obter a conta do banco de dados.");
                Assert.AreEqual(account.Login, retrievedAccount.Login, "Os logins não coincidem.");
                Assert.AreEqual(account.Password, retrievedAccount.Password, "As senhas não coincidem.");
                Assert.AreEqual(account.Email, retrievedAccount.Email, "Os e-mails não coincidem.");

                // Exemplo de como verificar a exclusão
                var specificAccountRepository = (AccountRepository)accountRepository;
                var login = "LoginAqui";
                //var result3 = await specificAccountRepository.DeleteByLoginAsync(login);

                //Assert.IsTrue(result3 > 0);
            }
        }
    }
}
