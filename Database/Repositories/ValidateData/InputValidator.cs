using Database.Entities.Account;
using SharedLibrary.Client;
using System.Text.RegularExpressions;

namespace Database.Repositories.ValidateData
{
    public static class InputValidator
    {
        public static OperationResult IsValidLogin(string login)
        {
            if (String.IsNullOrEmpty(login) || (login.Length > AccountEntity.MaxAccountCaracteres))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = $"Login out of range, Max caracteres is {AccountEntity.MaxAccountCaracteres}.",
                    ClientMessages = ClientMessages.UserLength,
                    Color = ConsoleColor.Red
                };

            }
            if (!Regex.IsMatch(login, "^[a-zA-Z0-9_]+$"))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "Valid Caracteres: a-z, A-Z, 0-9, _",
                    ClientMessages = ClientMessages.IllegalName,
                    Color = ConsoleColor.Red
                };
            }

            return new OperationResult { Success = true };
        }

        public static OperationResult IsValidPassword(string senha)
        {
            if (String.IsNullOrEmpty(senha) || (senha.Length > AccountEntity.MaxAccountCaracteres))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = $"Password out of range, Max caracteres is {AccountEntity.MaxAccountCaracteres}.",
                    ClientMessages = ClientMessages.WrongPass,
                    Color = ConsoleColor.Red
                };
            }

            return new OperationResult { Success = true };
        }

        public static OperationResult IsValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email) || (email.Length > AccountEntity.MaxEmailCaracteres))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = $"Email out of range, Max caracteres is {AccountEntity.MaxEmailCaracteres}.",
                    ClientMessages = ClientMessages.InvalidEmail
                };
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                if (addr.Address == email && email.Contains('@'))
                {
                    return new OperationResult { Success = true };
                }
            }
            catch
            {
                return new OperationResult
                {
                    Success = false,
                    Message = $"Email format invalid, format (####@#######.com)",
                    ClientMessages = ClientMessages.InvalidEmail, 
                    Color = ConsoleColor.Red,
                };
            }

            return new OperationResult { Success = true };
        }
    }
}
