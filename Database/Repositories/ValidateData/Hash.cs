using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories.ValidateData
{
    public class Hash
    {
        public static void CreatePasswordHash(string inputPassword, out string hashedPassword, out string salt)
        {
            // Geração de um salt aleatório
            byte[] saltBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            // Geração do hash usando PBKDF2
            using (var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32); // 32 bytes para um hash de 256 bits
                hashedPassword = Convert.ToBase64String(hashBytes);
            }
        }

        public static bool VerifyPassword(string inputPassword, string storedHash, string storedSalt)
        {
            // Convertendo o salt armazenado para bytes
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            // Calculando o hash da senha fornecida com o salt armazenado
            using (var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32); // 32 bytes para um hash de 256 bits
                string inputPasswordHash = Convert.ToBase64String(hashBytes);

                // Comparando o hash calculado com o hash armazenado
                return storedHash.Equals(inputPasswordHash);
            }
        }
    }
}
