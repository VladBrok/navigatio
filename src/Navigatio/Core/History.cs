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

    public void Push(string name, IExecutable command)
    {
        if (command is not ICancellable)
        {
            return;
        }

        // TODO: Remove code duplication with AliasesStorage class and Pop method
        Stack<(string, object)> history;
        if (!File.Exists(_file))
        {
            using var _ = File.Create(_file);
            history = new Stack<(string, object)>();
        }
        else
        {
            using var reader = new StreamReader(_file);
            string json = reader.ReadToEnd();
            history = new Stack<(string, object)>(
                JsonConvert.DeserializeObject<Stack<(string, object)>>(json)!);
        }

        history.Push((name, command));

        string resultJson = JsonConvert.SerializeObject(history);
        using var writer = new StreamWriter(_file);
        writer.WriteLine(resultJson);
    }

    public ICancellable? Pop(Func<string, IExecutable> getCommand)
    {
        Stack<(string, JObject)> history;

        using (var reader = new StreamReader(_file))
        {
            string json = reader.ReadToEnd();
            history = new Stack<(string, JObject)>(
                JsonConvert.DeserializeObject<Stack<(string, JObject)>>(json)!);
            if (!history.Any())
            {
                Console.WriteLine("Nothing to undo. Command history is empty.");
                return null;
            }
        }

        (string name, JObject data) = history.Pop();
        var command = (ICancellable)getCommand(name);
        JsonConvert.PopulateObject(data.ToString(), command);

        string resultJson = JsonConvert.SerializeObject(history);
        using var writer = new StreamWriter(_file);
        writer.WriteLine(resultJson);

        return command;
    }
}
