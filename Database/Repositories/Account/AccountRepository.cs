using Database.Entities.Account;
using Database.Entities.Player;
using Database.Repositories.ValidateData;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Client;


namespace Database.Repositories.Account
{
    public class AccountRepository : RepositoryBase<AccountEntity>
    {
        private readonly MeuDbContext _dbContext;

        public override object GetPrimaryKey(AccountEntity entity) => entity.Id;

        public AccountRepository(MeuDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> AddPlayerAccountAsync(AccountEntity account)
        {
            var login = InputValidator.IsValidLogin(account.Login);
            var password = InputValidator.IsValidPassword(account.Password);
            var email = InputValidator.IsValidEmail(account.Email);

            if (login.Success && password.Success && email.Success)
            {

                var verifyAccountExists = await CheckPlayerAccountAsync(account.Login);

                if (verifyAccountExists.Success == false)
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = $"[DATABASE] Account {account.Login} already exists!",
                        Color = ConsoleColor.Red,
                        ClientMenu = ClientMenu.MenuRegister,
                        ClientMessages = ClientMessages.NameTaken
                    };
                }

                //SHA256 Criptography of password -> Set on Database
                Hash.CreatePasswordHash(account.Password, out string passwordHash, out string salt);
                account.Password = passwordHash;
                account.Salt = salt;

                await _dbContext.AccountEntities.AddAsync(account);

                if (await _dbContext.SaveChangesAsync() > 0)
                {
                    return new OperationResult
                    {
                        Success = true,
                        Message = $"[DATABASE] Account {account.Login} has been addeded!",
                        Color = ConsoleColor.Green,
                        ClientMenu = ClientMenu.MenuLogin,
                        ClientMessages = ClientMessages.AccountCreated
                    };
                }
                else
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = $"[DATABASE] Account {account.Login} has not been addeded!",
                        Color = ConsoleColor.Red,
                        ClientMenu = ClientMenu.MenuMain,
                        ClientMessages = ClientMessages.Connection
                    };
                }
            }
            else // Tratamentos de erros
            {
                if (!login.Success)
                {
                    return login;
                }
                else if (!password.Success)
                {
                    return password;
                }
                else if (!email.Success)
                {
                    return email;
                }
                else
                {
                    return new OperationResult
                    {
                        Success = false,
                        Message = $"[DATABASE] Account {account.Login} has not been addeded!",
                        Color = ConsoleColor.Red,
                        ClientMenu = ClientMenu.MenuMain,
                        ClientMessages = ClientMessages.MySql
                    };
                }
            }
        }


        //Normalmente utilizado ao criar uma conta, verificando se existe alguma conta com o mesmo login
        //Esta sendo utilizado como login para testes!
        public async Task<OperationResult> CheckPlayerAccountAsync(string login)
        {
            var operationResult = new OperationResult();
            var conta = await _dbContext.AccountEntities
                .FirstOrDefaultAsync(e => e.Login == login);

            if (conta == null)
            {
                operationResult.Success = true;
                return operationResult;
            }
            else
            {
                operationResult.Success = false;
                operationResult.Message = $"[DATABASE] Account {login} already exists!";
                operationResult.Color = ConsoleColor.Red;
                operationResult.ClientMessages = ClientMessages.NameTaken;
                operationResult.ClientMenu = ClientMenu.MenuRegister;
                return operationResult;
            }
        }

        public async Task<CombinedOperationResult<AccountEntity>> AuthenticateAccount(string login, string password)
        {
            var combinedOperations = new CombinedOperationResult<AccountEntity>();

            var conta = await _dbContext.AccountEntities
                .FirstOrDefaultAsync(e => e.Login == login);

            combinedOperations.Entity = conta;

            if (combinedOperations.Entity == null)
            {
                combinedOperations.Success = false;
                combinedOperations.Message = $"[DATABASE] Account {login} not found!";
                combinedOperations.Color = ConsoleColor.Red;
                combinedOperations.ClientMessages = ClientMessages.WrongPass;
                combinedOperations.ClientMenu = ClientMenu.MenuLogin;
                return combinedOperations;

            }

            if (Hash.VerifyPassword(password, conta.Password, conta.Salt))
            {
                combinedOperations.Success = true;
                combinedOperations.Message = $"[DATABASE] Account {login} has been authenticated!";
                combinedOperations.Color = ConsoleColor.Green;
            }
            else
            {
                combinedOperations.Success = false;
                combinedOperations.Message = $"[DATABASE] Account {login} has not been authenticated!";
                combinedOperations.Color = ConsoleColor.Red;
                combinedOperations.ClientMessages = ClientMessages.WrongPass;
                combinedOperations.ClientMenu = ClientMenu.MenuLogin;
            }

            return combinedOperations;
            
        }

        //Normalmente utilizado para envio da conta ao servidor do jogo
        public async Task<AccountEntity?> GivePlayerAccountAsync(string login, string password)
        {
            var conta = await _dbContext.AccountEntities
                .FirstOrDefaultAsync(e => e.Login == login);

            if (conta == null) { return null; }

            if (Hash.VerifyPassword(password, conta.Salt, conta.Password))
            {
                return conta;
            }

            return null;
        }

        public async Task<int> AtualizarContaAsync()
        {
            try
            {
                var result = await _dbContext.SaveChangesAsync();

                return result; // Retorna o número de entidades atualizadas
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao atualizar conta: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> ExcluirContaPorLoginAsync(string login)
        {
            var accounts = await _dbContext.AccountEntities
                .Where(e => e.Login == login)
                .ToListAsync();

            if (accounts.Any())
            {
                _dbContext.AccountEntities.RemoveRange(accounts);
                await _dbContext.SaveChangesAsync();
                return accounts.Count;
            }

            return 0;
        }
    }
}
