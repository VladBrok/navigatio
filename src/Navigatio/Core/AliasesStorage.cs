using System.Text.Json;

namespace Navigatio;

public class AliasesStorage
{
    private readonly string _file;

    public AliasesStorage(string file)
    {
        _file = file;
    }

    public void Save(Dictionary<string, string> aliases)
    {
        string json = JsonSerializer.Serialize(aliases);
        using var writer = new StreamWriter(_file);
        writer.WriteLine(json);
    }

    public Dictionary<string, string> Load()
    {
        // TODO: remove code duplication with History.cs
        if (!File.Exists(_file))
        {
            using var _ = File.Create(_file);
            return new Dictionary<string, string>();
        }

        using var reader = new StreamReader(_file);
        string json = reader.ReadToEnd();
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
    }
}
