namespace Navigatio.Storages;

public interface IStorage<T>
    where T : notnull, new()
{
    void Load(Action<T> callback, bool modifiesData = true);
}
