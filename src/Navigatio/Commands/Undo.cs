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
        string? commandName = null;
        ICancellable? last = _history.Pop(name =>
        {
            CommandData? c = _commander.Get(name);
            Debug.Assert(c is not null);

            commandName = c.Name;
            return c.Executor();
        });

        if (last is null)
        {
            Console.WriteLine("Nothing to undo. Command history is empty.");
            return false;
        }

        Console.WriteLine($"Undoing the command '{commandName}'...");
        last.Cancel();
        return true;
    }
}
