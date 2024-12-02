using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_git.src.Utils
{
    public class PathHelper
    {
        public static string GetGitObjectPath(string hash)
        {
            return Path.Combine(".git", "objects", hash[..2], hash[2..]);
        }

        public static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static void CreateFileFromHash(string hash, byte[] data)
        {
            string folder = new String(hash.ToCharArray()[..2]);
            string file = new String(hash.ToCharArray()[2..]);
            string gitObjectsPath = Path.Combine(".git", "objects");
            string folderPath = Path.Combine(gitObjectsPath, folder);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string filePath = Path.Combine(folderPath, file);
            using (var zs = new ZLibStream(
                       new FileStream(filePath, FileMode.Create, FileAccess.Write),
                       CompressionMode.Compress))
            {
                zs.Write(data);
            }

        }
    }
}
