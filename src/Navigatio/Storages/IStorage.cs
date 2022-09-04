namespace Navigatio.Storages;

public interface IStorage<T> where T : notnull, new()
{
    string File { get; }
    void Load(Action<T> callback, bool modifiesData = true);
}
