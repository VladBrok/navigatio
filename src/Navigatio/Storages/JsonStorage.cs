using Newtonsoft.Json;

namespace Navigatio.Storages;

public class JsonStorage : IStorage
{
    private readonly string _file;
    private readonly JsonSerializerSettings _settings;

    public JsonStorage(string file)
    {
        _file = file;
        _settings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    public void Load<T>(Action<T> callback)
        where T : new()
    {
        var data = Load<T>();
        callback(data);
        Save(data);
    }

    private T Load<T>()
        where T : new()
    {
        if (!File.Exists(_file))
        {
            using var _ = File.Create(_file);
            return new T();
        }

        using var reader = new StreamReader(_file);
        string json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(json, _settings)!;
    }

    private void Save<T>(T data)
    {
        string json = JsonConvert.SerializeObject(data, _settings);
        using var writer = new StreamWriter(_file);
        writer.WriteLine(json);
    }
}
