namespace Navigatio.Commands;

public class UndoCommand : IExecutable
{
    private readonly Commander _commander;
    private readonly History _history;

    public UndoCommand(Commander commander, History history)
    {
        _commander = commander;
        _history = history;
    }

    public void Execute(params string[] _)
    {
        _history.Pop(_commander.Get)?.Cancel();
    }
}
