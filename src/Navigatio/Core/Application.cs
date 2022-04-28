using Navigatio.Commands;

namespace Navigatio;

public class Application
{
    private string[] _args;
    private readonly Commander _commander;
    private readonly History _history;

    public Application(string[] args, Commander commander, History history)
    {
        _args = args;
        _commander = commander;
        _history = history;
    }

    public void Run()
    {
        IExecutable command = _commander.Get(ref _args);
        command.Execute(_args[1..]);
        _history.Push(_args[0], command);
    }
}
