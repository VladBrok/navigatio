namespace Navigatio.Storages;

public interface IStorage
{
    void Load<T>(Action<T> callback)
        where T : new();
}
