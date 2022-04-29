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
        _aliasStorage.Load<Dictionary<string, string>>(aliases =>
        {
            foreach (var alias in aliases)
            {
                Console.WriteLine($"  {alias.Key}  ->  {alias.Value}");
            }
        });
        return true;
    }
}
