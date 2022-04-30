using Navigatio.Storages;

namespace Navigatio.Commands;

public class Show : IExecutable
{
    private readonly IStorage<Dictionary<string, string>> _aliasStorage;

    public Show(IStorage<Dictionary<string, string>> aliasStorage)
    {
        _aliasStorage = aliasStorage;
    }

    public bool Execute(params string[] _)
    {
        Console.WriteLine();
        _aliasStorage.Load(aliases =>
        {
            int maxWidth = aliases.Keys.Max(x => x.Length) + 2;
            foreach (var alias in aliases)
            {
                Console.WriteLine($"{alias.Key.PadLeft(maxWidth)}   ->   {alias.Value}");
            }
        });
        return true;
    }
}
