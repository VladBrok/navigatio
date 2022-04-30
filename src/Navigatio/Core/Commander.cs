using Navigatio.Commands;
using Navigatio.Storages;

namespace Navigatio;

public class Commander
{
    private readonly Dictionary<string, Func<IExecutable>> _commands;

    public Commander(IStorage<Dictionary<string, string>> aliases, History history, string shellOutputFile)
    {
        _commands = new Dictionary<string, Func<IExecutable>>
        {
            ["--add"] = () => new Add(aliases),
            ["--del"] = () => new Delete(aliases),
            ["--move"] = () => new Move(shellOutputFile, aliases),
            ["--show"] = () => new Show(aliases),
            ["--undo"] = () => new Undo(this, history),
        };
    }

    public IExecutable Get(string name)
    {
        return _commands[name]();
    }

    public IExecutable Get(ref string[] args)
    {
        if (_commands.TryGetValue(args[0], out Func<IExecutable>? c))
        {
            return c();
        }

        args = args.Prepend("--move").ToArray();
        return _commands["--move"]();
    }
}
