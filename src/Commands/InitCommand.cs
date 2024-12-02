using codecrafters_git.src.Commands.Interfaces;

namespace codecrafters_git.src.Commands
{
    public class InitCommand : ICommand
    {
        public void Execute(string[] args)
        {
            Directory.CreateDirectory(".git");
            Directory.CreateDirectory(".git/objects");
            Directory.CreateDirectory(".git/refs");
            File.WriteAllText(".git/HEAD", "ref: refs/heads/main\n");
            Console.WriteLine("Initialized git directory");
        }
    }
}
