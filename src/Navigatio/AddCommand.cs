using System.Diagnostics;
using System.Text.Json;

public class AddCommand : IExecutable, ICancellable
{
    private readonly string _file = "aliases.json";

    public string? Alias { get; set; }
    public string? OldPath { get; set; }

    public void Execute(params string[] args)
    {
        if (args.Length < 2 || args[0].StartsWith("-"))
        {
            throw new CommandUsageException();
        }

        string path = args[1];
        try
        {
            Directory.CreateDirectory(path);
        }
        catch
        {
            Console.WriteLine($"Path '{path}' is invalid.");
            return;
        }

        Dictionary<string, string> aliases = LoadAliases();
        string alias = args[0];

        if (!aliases.TryAdd(alias, path))
        {
            OldPath = aliases[alias];
            aliases[alias] = path;
        }

        Alias = alias;
        Save(aliases);
    }

    public void Cancel()
    {
        Debug.Assert(Alias is not null);

        Dictionary<string, string> aliases = LoadAliases();
        if (OldPath is null)
        {
            aliases.Remove(Alias);
        }
        else
        {
            aliases[Alias] = OldPath;
        }
        Save(aliases);
    }

    private void Save(Dictionary<string, string> aliases)
    {
        var writer = new StreamWriter(_file);
        string resultJson = JsonSerializer.Serialize<Dictionary<string, string>>(aliases);
        writer.WriteLine(resultJson);
    }

    private Dictionary<string, string> LoadAliases()
    {
        var aliases = new Dictionary<string, string>();
        using var reader = new StreamReader(_file);
        string json = reader.ReadToEnd();
        if (!string.IsNullOrEmpty(json))
        {
            aliases = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
        }
        return aliases;
    }
}
