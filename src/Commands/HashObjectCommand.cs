using codecrafters_git.src.Commands.Interfaces;
using codecrafters_git.src.Services;
using codecrafters_git.src.Utils;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;


namespace codecrafters_git.src.Commands
{
    public class HashObjectCommand : ICommand
    {
        private readonly GitObjectStore _gitObjectStore;
        private readonly HashCalculator _hashCalculator;

        public HashObjectCommand()
        {
            _gitObjectStore = new GitObjectStore();
            _hashCalculator = new HashCalculator();
        }
        public void Execute(string[] args)
        {
            if (args.Length < 3 || args[1] != "-w")
                throw new ArgumentException("Invalid arguments for hash-object");

            string fileName = args[2];
            Validation.ValidateFileExists(fileName);

            byte[] fileContent = File.ReadAllBytes(fileName);
            string header = $"blob {fileContent.Length}\0";
            byte[] objectBytes = Encoding.UTF8.GetBytes(header).Concat(fileContent).ToArray();

            string hash = _hashCalculator.ComputeSHA1(objectBytes);
            _gitObjectStore.SaveObject(objectBytes, hash);

            Console.WriteLine(hash);

        }
    }
}
