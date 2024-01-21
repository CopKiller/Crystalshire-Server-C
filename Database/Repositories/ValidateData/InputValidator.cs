using Database.Client;
using Database.Entities.Account;
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
                    Message = $"[DATABASE] Login out of range, Max caracteres is {AccountEntity.MaxAccountCaracteres}.",
                    ClientMSG = ClientMessages.UserLength,
                    Color = ConsoleColor.Red
                };

            }
            if (!Regex.IsMatch(login, "^[a-zA-Z0-9_]+$"))
            {
                return new OperationResult
                {
                    Success = false,
                    Message = "[DATABASE] Valid Caracteres: a-z, A-Z, 0-9, _",
                    ClientMSG = ClientMessages.IllegalName,
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
                    Message = $"[DATABASE] Password out of range, Max caracteres is {AccountEntity.MaxAccountCaracteres}.",
                    ClientMSG = ClientMessages.WrongPass,
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
                    Message = $"[DATABASE] Email out of range, Max caracteres is {AccountEntity.MaxEmailCaracteres}.",
                    ClientMSG = ClientMessages.IllegalName};
            }

            var addr = new System.Net.Mail.MailAddress(email);

            if (addr.Address == email && email.Contains('@'))
            {
                return new OperationResult { Success = true };
            }
            else
            {
                return new OperationResult 
                { 
                    Success = false, 
                    Message = $"[DATABASE] Email format invalid, format (####@#######.com)",
                    ClientMSG = ClientMessages.IllegalName};
            }
        }
    }
}
