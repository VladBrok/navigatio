using Navigatio.Commands;
using Navigatio.Storages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Navigatio;

public class History
{
    private readonly IStorage _storage;

    public History(IStorage storage)
    {
        _storage = storage;
    }

    public void Push(string name, ICancellable command)
    {
        var history = LoadStack<(string, object)>();
        history.Push((name, command));
        _storage.Save(history);
    }

    public ICancellable? Pop(Func<string, IExecutable> getCommand)
    {
        var history = LoadStack<(string, JObject)>();
        if (!history.Any())
        {
            return null;
        }

        (string name, JObject data) = history.Pop();
        var command = (ICancellable)getCommand(name);
        JsonConvert.PopulateObject(data.ToString(), command);

        _storage.Save(history);
        return command;
    }

    private Stack<T> LoadStack<T>()
    {
        // Need to wrap in the new stack, because otherwise
        // it loads in the reverse order.
        return new Stack<T>(_storage.Load<Stack<T>>());
    }
}
