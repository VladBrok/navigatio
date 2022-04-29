using Navigatio.Storages;

namespace Navigatio.Commands;

public class Show : IExecutable
{
    private readonly IStorage _aliasStorage;

    public Show(IStorage aliasStorage)
    {
        _aliasStorage = aliasStorage;
    }

    public bool Execute(params string[] _)
    {
        Console.WriteLine();
        foreach (var alias in _aliasStorage.Load<Dictionary<string, string>>())
        {
            Console.WriteLine($"  {alias.Key}  ->  {alias.Value}");
        }
        return true;
    }
}
