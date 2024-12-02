using codecrafters_git.src.Commands.Interfaces;
using codecrafters_git.src.Services;
using codecrafters_git.src.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace codecrafters_git.src.Commands
{
    public class WriteTreeCommand : ICommand
    {
        private readonly GitObjectStore _gitObjectStore; // To handle storing objects
        private readonly HashCalculator _hashCalculator;   // To compute SHA-1 hashes

        public WriteTreeCommand()
        {
            _gitObjectStore = new GitObjectStore();
            _hashCalculator = new HashCalculator();
        }
        public void Execute(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            (string hash, byte[] _) = WriteTreeObject(currentDirectory);
            Console.Write(hash);
        }
        private static (string, byte[]) WriteTreeObject(string directory)
        {
            IEnumerable<string> directories =
                Directory.GetDirectories(directory).Where(
                    dir => !dir.EndsWith(Path.DirectorySeparatorChar + ".git"));
            string[] files = Directory.GetFiles(directory);
            IOrderedEnumerable<string> entries = directories.Concat(files).Order();
            byte[] treeObjectBytes;
            using (MemoryStream treeObjectBody = new MemoryStream())
            {
                foreach (string? entry in entries)
                {
                    byte[] rowBytes;
                    if (Directory.Exists(entry))
                    {
                        (string _, byte[] treeHashRow) = WriteTreeObject(entry);
                        string treeObjectRow = $"40000 {Path.GetFileName(entry)}\0";
                        byte[] treeObjectRowBytes =
                            Encoding.UTF8.GetBytes(treeObjectRow);
                        rowBytes = [.. treeObjectRowBytes, .. treeHashRow];
                    }
                    else
                    {
                        (string _, byte[] blobHashRow) = WriteBlobObject(entry);
                        string blobObjectRow =
                            $"100644 {Path.GetFileName(entry)}\0";
                        byte[] blobObjectRowBytes =
                            Encoding.UTF8.GetBytes(blobObjectRow);
                        rowBytes = [.. blobObjectRowBytes, .. blobHashRow];
                    }
                    treeObjectBody.Write(rowBytes, 0, rowBytes.Length);
                }
                string treeObjectHeader = $"tree {treeObjectBody.Length}\0";
                byte[] treeObjectHeaderBytes =
                    Encoding.UTF8.GetBytes(treeObjectHeader);
                treeObjectBytes =
                    [.. treeObjectHeaderBytes, .. treeObjectBody.ToArray()];
            }
            string treeHash = BitConverter.ToString(SHA1.HashData(treeObjectBytes))
                                  .Replace("-", "")
                                  .ToLower();
            string treeObjectPath =
                Path.Combine(".git", "objects", treeHash[..2], treeHash[2..]);
            Directory.CreateDirectory(Path.GetDirectoryName(treeObjectPath)!);
            PathHelper.CreateFileFromHash(treeHash, treeObjectBytes);
            return (treeHash, SHA1.HashData(treeObjectBytes));
        }
        private static (string, byte[]) WriteBlobObject(string directory)
        {
            byte[] fileContent = File.ReadAllBytes(directory);
            string blobHeader = $"blob {fileContent.Length}\0";
            byte[] blobHeaderBytes = Encoding.UTF8.GetBytes(blobHeader);
            byte[] blobObjectBytes = [.. blobHeaderBytes, .. fileContent];
            string blobHash = BitConverter.ToString(SHA1.HashData(blobObjectBytes))
                                  .Replace("-", "")
                                  .ToLower();
            string blobObjectPath =
                Path.Combine(".git", "objects", blobHash[..2], blobHash[2..]);
            Directory.CreateDirectory(Path.GetDirectoryName(blobObjectPath)!);
            PathHelper.CreateFileFromHash(blobHash, blobObjectBytes);
            return (blobHash, SHA1.HashData(blobObjectBytes));
        }

    }
}
