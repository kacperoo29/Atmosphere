using System.Security.Cryptography;
using System.Text;

namespace Atmosphere.Core;

public static class PasswordUtil 
{
    public static byte[] GenerateSaltedHash(byte[] text)
    {
        var salt = GetSalt();
        using HashAlgorithm algorithm = SHA256.Create();
        var plainTextWithSalt = new byte[text.Length + salt.Length];
        Buffer.BlockCopy(text, 0, plainTextWithSalt, 0, text.Length);
        Buffer.BlockCopy(salt, 0, plainTextWithSalt, text.Length, salt.Length);

        return algorithm.ComputeHash(plainTextWithSalt);
    }

    public static bool VerifyPassword(string password, byte[] hash)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] passwordHash = GenerateSaltedHash(passwordBytes);

        return passwordHash.SequenceEqual(hash);
    }

    public static byte[] GetSalt()
    {
        return Encoding.UTF8.GetBytes("DhYaQMN2UXtFVbPW9vnMLCP7");
    }
}