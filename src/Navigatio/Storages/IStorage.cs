namespace Navigatio.Storages;

public interface IStorage
{
    void Load<T>(Action<T> callback)
        where T : notnull, new();
}
