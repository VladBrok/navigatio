using Newtonsoft.Json;

namespace Navigatio.Storages;

public class JsonStorage : IStorage
{
    private readonly string _file;

    public JsonStorage(string file)
    {
        _file = file;
    }

    public void Save<T>(T data)
    {
        string json = JsonConvert.SerializeObject(data);
        using var writer = new StreamWriter(_file);
        writer.WriteLine(json);
    }

    public T Load<T>() where T : new()
    {
        if (!File.Exists(_file))
        {
            using var _ = File.Create(_file);
            return new T();
        }

        using var reader = new StreamReader(_file);
        string json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(json)!;
    }
}
