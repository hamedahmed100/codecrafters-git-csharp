using System.IO.Compression;
using System.Text;
using codecrafters_git.src.Commands.Interfaces;
using codecrafters_git.src.Services;


namespace codecrafters_git.src.Commands
{
    public class CatFileCommand : ICommand
    {
        private readonly GitObjectStore _gitObjectStore;

        public CatFileCommand()
        {
            _gitObjectStore = new GitObjectStore();
        }
        public void Execute(string[] args)
        {
            if (args.Length < 3 || args[1] != "-p")
                throw new ArgumentException("Invalid arguments for cat-file");

            string objectHash = args[2];

            try
            {
                // Fetch and decompress the object
                byte[] objectData = _gitObjectStore.ReadObject(objectHash);

                // Parse the object
                string[] metaDataAndContent = Encoding.UTF8.GetString(objectData).Split('\0', 2);
                if (metaDataAndContent.Length < 2)
                    throw new Exception("Invalid object format!");

                string objectContent = metaDataAndContent[1];

                // Output the content
                Console.Write(objectContent);
            }
            catch (FileNotFoundException)
            {
                throw new Exception($"Object {objectHash} not found.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
