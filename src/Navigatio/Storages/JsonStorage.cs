using Newtonsoft.Json;

namespace Navigatio.Storages;

public class JsonStorage<T> : IStorage<T>, IPopulator<T>
    where T : notnull, new()
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

    public void Load(Action<T> callback, bool modifiesData = true)
    {
        T? data = default;
        try
        {
            data = Load();
            callback(data);
        }
        finally
        {
            if (modifiesData)
            {
                Save(data);
            }
        }
    }

    public void Populate<S, U>(S obj, U data)
        where S : notnull
        where U : notnull
    {
        JsonConvert.PopulateObject(data.ToString() ?? "", obj);
    }

    private T Load()
    {
        if (!File.Exists(_file))
        {
            using var _ = File.Create(_file);
            return new T();
        }

        using var reader = new StreamReader(_file);
        string json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(json, _settings) ?? new T();
    }

    private void Save(T? data)
    {
        string json = JsonConvert.SerializeObject(data, _settings);
        using var writer = new StreamWriter(_file);
        writer.WriteLine(json);
    }
}
