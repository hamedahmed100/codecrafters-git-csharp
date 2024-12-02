using codecrafters_git.src.Commands;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 1)
        {
            Console.WriteLine("Please provide a command.");
            return;
        }

        string commandName = args[0];
        try
        {
            var command = CommandFactory.GetCommand(commandName);
            command.Execute(args);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}

