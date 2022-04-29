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
        _storage.Load<LinkedList<(string, object)>>(history =>
        {
            history.AddFirst((name, command));
        });
    }

    public ICancellable? Pop(Func<string, IExecutable> getCommand)
    {
        ICancellable? command = null;
        _storage.Load<LinkedList<(string, JObject)>>(history =>
        {
            if (!history.Any())
            {
                return;
            }

            (string name, JObject data) = history.First();
            history.RemoveFirst();
            command = (ICancellable)getCommand(name);
            JsonConvert.PopulateObject(data.ToString(), command);
        });

        return command;
    }
}
