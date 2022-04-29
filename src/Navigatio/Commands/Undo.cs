namespace Navigatio.Commands;

public class Undo : IExecutable
{
    private readonly Commander _commander;
    private readonly History _history;

    public Undo(Commander commander, History history)
    {
        _commander = commander;
        _history = history;
    }

    public bool Execute(params string[] _)
    {
        ICancellable? last = _history.Pop(_commander.Get);
        if (last is null)
        {
            Console.WriteLine("Nothing to undo. Command history is empty.");
            return false;
        }

        last.Cancel();
        return true;
    }
}
