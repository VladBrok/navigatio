using Navigatio.Commands;

namespace Navigatio;

public class Commander
{
    private readonly Dictionary<string, IExecutable> _commands;

    public Commander(AliasesStorage storage, History history)
    {
        _commands = new Dictionary<string, IExecutable>
        {
            ["--add"] = new AddCommand(storage),
            ["--del"] = new DeleteCommand(storage),
            ["--move"] = new MoveCommand(outputFile: "output.sh", storage),
            ["--show"] = new ShowCommand(storage),
            ["--undo"] = new UndoCommand(this, history),
        };
    }

    public IExecutable Get(string name)
    {
        return _commands[name];
    }

    public IExecutable Get(ref string[] args)
    {
        if (_commands.TryGetValue(args[0], out IExecutable? c))
        {
            return c;
        }

        args = args.Prepend("--move").ToArray();
        return _commands["--move"];
    }
}
