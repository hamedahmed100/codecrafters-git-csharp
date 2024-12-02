using codecrafters_git.src.Commands.Interfaces;

namespace codecrafters_git.src.Commands
{
    public class CommandFactory
    {
        private static readonly Dictionary<string, Func<ICommand>> Commands = new();
        static CommandFactory()
        {
            RegisterCommand("init", () => new InitCommand());
            RegisterCommand("cat-file", () => new CatFileCommand());
            RegisterCommand("hash-object", () => new HashObjectCommand());
            RegisterCommand("ls-tree", () => new LsTreeNameOnlyCommand());
            RegisterCommand("write-tree", () => new WriteTreeCommand());
            RegisterCommand("commit-tree", () => new CommitTreeCommand());
            RegisterCommand("clone", () => new CloneCommand());
        }
        public static void RegisterCommand(string commandName, Func<ICommand> commandCreator)
        {
            if (Commands.ContainsKey(commandName))
                throw new InvalidOperationException($"Command {commandName} is already registered.");

            Commands[commandName] = commandCreator;
        }

        public static ICommand GetCommand(string command)
        {
            if (!Commands.TryGetValue(command, out var commandCreator))
                throw new ArgumentException($"Unknown command {command}");

            return commandCreator();
        }
    }
}
