using System.Diagnostics;

namespace Navigatio.Commands;

public class Undo : IExecutable
{
    private readonly ICommander _commander;
    private readonly History _history;

    public Undo(ICommander commander, History history)
    {
        _commander = commander;
        _history = history;
    }

    public bool Execute(params string[] _)
    {
        ICancellable? last = _history.Pop(name =>
        {
            IExecutable? c = _commander.Get(name)?.Executor();
            Debug.Assert(c is not null);
            return c;
        });

        if (last is null)
        {
            Console.WriteLine("Nothing to undo. Command history is empty.");
            return false;
        }

        last.Cancel();
        return true;
    }
}
