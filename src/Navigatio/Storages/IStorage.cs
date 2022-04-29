namespace Navigatio.Storages;

public interface IStorage
{
    void Save<T>(T data);
    T Load<T>() where T : new();
}
