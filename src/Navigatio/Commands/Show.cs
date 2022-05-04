using Navigatio.Storages;

namespace Navigatio.Commands;

public class Show : IExecutable
{
    private readonly IStorage<Dictionary<string, string>> _aliasStorage;
    private readonly Table _table;

    public Show(IStorage<Dictionary<string, string>> aliasStorage, Table table)
    {
        _aliasStorage = aliasStorage;
        _table = table;
    }

    public bool Execute(params string[] _)
    {
        Console.WriteLine();
        _aliasStorage.Load(aliases =>
        {
            if (!aliases.Any())
            {
                Console.WriteLine("Aliases not found.");
                return;
            }
            _table.Print(
                aliases.Keys,
                aliases.Values,
                columnGap: 4,
                marginLeft: 2);
        }, modifiesData: false);
        return true;
    }
}
