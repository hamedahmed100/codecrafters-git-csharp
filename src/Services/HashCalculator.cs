using System.Security.Cryptography;

namespace codecrafters_git.src.Services
{
    public class HashCalculator
    {
        public string ComputeSHA1(byte[] data)
        {
            using SHA1 sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(data);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
