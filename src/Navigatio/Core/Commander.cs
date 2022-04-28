using Navigatio.Commands;

namespace Navigatio;

public class Commander
{
    private readonly Dictionary<string, IExecutable> _commands;

    public Commander(Aliases aliases, History history, string shellOutputFile)
    {
        _commands = new Dictionary<string, IExecutable>
        {
            ["--add"] = new Add(aliases),
            ["--del"] = new Delete(aliases),
            ["--move"] = new Move(shellOutputFile, aliases),
            ["--show"] = new Show(aliases),
            ["--undo"] = new Undo(this, history),
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
