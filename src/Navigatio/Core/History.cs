using Navigatio.Commands;
using Navigatio.Storages;

namespace Navigatio;

public class History
{
    private readonly IPopulator<LinkedList<(string, object)>> _storage;
    private readonly int _limit;

    public History(IPopulator<LinkedList<(string, object)>> storage, int limit)
    {
        _storage = storage;
        _limit = limit;
    }

    public void Push(string name, ICancellable command)
    {
        _storage.Load(
            history =>
            {
                history.AddFirst((name, command));
                EnsureLimited(history);
            }
        );
    }

    public ICancellable? Pop(Func<string, IExecutable> getCommand)
    {
        ICancellable? command = null;
        _storage.Load(
            history =>
            {
                if (!history.Any())
                {
                    return;
                }

                (string name, object data) = history.First();
                history.RemoveFirst();
                command = (ICancellable)getCommand(name);
                _storage.Populate(command, data);
            }
        );

        return command;
    }

    private void EnsureLimited(LinkedList<(string, object)> history)
    {
        while (history.Count > _limit)
        {
            history.RemoveLast();
        }
    }
}
