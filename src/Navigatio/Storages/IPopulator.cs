namespace Navigatio.Storages;

public interface IPopulator : IStorage
{
    void Populate<T, U>(T target, U data)
        where T : notnull
        where U : notnull;
}
