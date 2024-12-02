
using System.IO.Compression;

namespace codecrafters_git.src.Services
{
    public class GitObjectStore
    {
        public void SaveObject(byte[] objectBytes, string hash)
        {
            string objectDir = Path.Combine(".git", "objects", hash[..2]);
            string objectFile = Path.Combine(objectDir, hash[2..]);

            Directory.CreateDirectory(objectDir);

            using FileStream fileStream = File.Create(objectFile);
            using ZLibStream zLibStream = new(fileStream, CompressionMode.Compress);
            zLibStream.Write(objectBytes, 0, objectBytes.Length);
        }

        public byte[] ReadObject(string hash)
        {
            string objectDir = Path.Combine(".git", "objects", hash[..2]);
            string objectFile = Path.Combine(objectDir, hash[2..]);

            if (!File.Exists(objectFile))
                throw new FileNotFoundException($"Object {hash} not found.");

            using FileStream fileStream = File.OpenRead(objectFile);
            using ZLibStream zLibStream = new(fileStream, CompressionMode.Decompress);
            using MemoryStream uncompressedStream = new();
            zLibStream.CopyTo(uncompressedStream);
            return uncompressedStream.ToArray();
        }
    }
}
