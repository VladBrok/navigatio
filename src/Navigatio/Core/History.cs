using Navigatio.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Navigatio;

public class History
{
    private readonly string _file;

    public History(string file)
    {
        _file = file;
    }

    public void Push(string name, ICancellable command)
    {
        Stack<(string, object)> history = Load<object>();
        history.Push((name, command));
        Save(history);
    }

    public ICancellable? Pop(Func<string, IExecutable> getCommand)
    {
        Stack<(string, JObject)> history = Load<JObject>();
        if (!history.Any())
        {
            return null;
        }

        (string name, JObject data) = history.Pop();
        var command = (ICancellable)getCommand(name);
        JsonConvert.PopulateObject(data.ToString(), command);

        Save(history);
        return command;
    }

    // TODO: Remove code duplication with Aliases class
    private void Save<T>(Stack<(string, T)> history)
    {
        string json = JsonConvert.SerializeObject(history);
        using var writer = new StreamWriter(_file);
        writer.WriteLine(json);
    }

    private Stack<(string, T)> Load<T>()
    {
        if (!File.Exists(_file))
        {
            using var _ = File.Create(_file);
            return new Stack<(string, T)>();
        }

        using var reader = new StreamReader(_file);
        string json = reader.ReadToEnd();
        return new Stack<(string, T)>(
            JsonConvert.DeserializeObject<Stack<(string, T)>>(json)!);
    }
}
