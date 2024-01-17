using System.Security.Cryptography;
using System.Text;

namespace SharedLibrary.Communication;

public sealed class KeyGenerator
{
    private const string Words = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    private const int Size = 15;

    public string GetUniqueKey()
    {
        var chars = Words.ToCharArray();
        var data = new byte[Size];

        using (var crypto = new RNGCryptoServiceProvider())
        {
            crypto.GetBytes(data);
        }

        var result = new StringBuilder(Size);

        foreach (var b in data) result.Append(chars[b % chars.Length]);

        return result.ToString();
    }
}