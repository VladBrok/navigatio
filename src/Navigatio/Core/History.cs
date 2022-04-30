using Navigatio.Commands;
using Navigatio.Storages;

namespace Navigatio;

public class History
{
    private readonly IPopulator _storage;

    public History(IPopulator storage)
    {
        _storage = storage;
    }

    public void Push(string name, ICancellable command)
    {
        _storage.Load<LinkedList<(string, object)>>(history =>
        {
            history.AddFirst((name, command));
        });
    }

    public ICancellable? Pop(Func<string, IExecutable> getCommand)
    {
        ICancellable? command = null;
        _storage.Load<LinkedList<(string, object)>>(history =>
        {
            if (!history.Any())
            {
                return;
            }

            (string name, object data) = history.First();
            history.RemoveFirst();
            command = (ICancellable)getCommand(name);
            _storage.Populate(command, data);
        });

        return command;
    }
}
