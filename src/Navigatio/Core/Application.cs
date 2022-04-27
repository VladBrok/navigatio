using Navigatio.Commands;

namespace Navigatio;

public class Application
{
    private readonly Dictionary<string, IExecutable> _commands;
    private string[] _args;

    public Application(string[] args)
    {
        Directory.SetCurrentDirectory(AppContext.BaseDirectory);

        _args = args;
        var storage = new AliasesStorage("aliases.json");
        _commands = new Dictionary<string, IExecutable>
        {
            ["--add"] = new AddCommand(storage),
            ["--del"] = new DeleteCommand(storage),
            ["--move"] = new MoveCommand(outputFile: "output.sh", storage),
            ["--show"] = new ShowCommand(storage),
            // ["--undo"] = new UndoCommand(),
        };
    }

    public void Run()
    {
        if (!_args.Any())
        {
            Console.WriteLine("no args.");
            return;
        }

        IExecutable command = GetCommand(_args[0]);
        command.Execute(_args[1..]);
    }

    public IExecutable GetCommand(string name)
    {
        if (_commands.TryGetValue(name, out IExecutable? c))
        {
            return c;
        }

        _args = _args.Prepend("--move").ToArray();
        return _commands["--move"];
    }
}
