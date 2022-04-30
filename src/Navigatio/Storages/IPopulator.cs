namespace Navigatio.Storages;

public interface IPopulator<T> : IStorage<T>
    where T : notnull, new()
{
    void Populate<S, U>(S target, U data)
        where S : notnull
        where U : notnull;
}
