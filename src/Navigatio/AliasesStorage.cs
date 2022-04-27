using System.Text.Json;

public class AliasesStorage
{
    private readonly string _file;

    public AliasesStorage(string file)
    {
        _file = file;
        if (!File.Exists(file))
        {
            using var _ = File.Create(file);
        }
    }

    public void Save(Dictionary<string, string> aliases)
    {
        string json = JsonSerializer.Serialize<Dictionary<string, string>>(aliases);
        using var writer = new StreamWriter(_file);
        writer.WriteLine(json);
    }

    public Dictionary<string, string> Load()
    {
        using var reader = new StreamReader(_file);
        string json = reader.ReadToEnd();
        return string.IsNullOrEmpty(json)
               ? new Dictionary<string, string>()
               : JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;
    }
}
