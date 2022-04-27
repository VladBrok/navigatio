public class Application
{
    private readonly Dictionary<string, IExecutable> _commands;

    public Application()
    {
        _commands = new Dictionary<string, IExecutable>
        {
            ["--add"] = new AddCommand()
            //["--del"] = new DeleteCommand(),
            //["--show"] = new ShowCommand()
        };
    }

    public void Run(string[] args)
    {
        if (!args.Any())
        {
            Console.WriteLine("no args.");
            return;
        }

        IExecutable command = _commands[args[0]];
        command.Execute(args[1..]);
    }
}
