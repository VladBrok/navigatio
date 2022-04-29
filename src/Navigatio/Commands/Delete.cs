using Navigatio.Storages;

namespace Navigatio.Commands;

public class Delete : IExecutable, ICancellable
{
    private readonly IStorage _aliasStorage;

    public Delete(IStorage aliasStorage)
    {
        _aliasStorage = aliasStorage;
    }

    public string? Alias { get; set; }
    public string? Path { get; set; }

    public bool Execute(params string[] args)
    {
        if (args.Length < 1)
        {
            throw new CommandUsageException();
        }

        string alias = args[0];
        var aliases = _aliasStorage.Load<Dictionary<string, string>>();
        if (!aliases.ContainsKey(alias))
        {
            Console.WriteLine($"Alias '{alias}' not found.");
            return false;
        }

        Alias = alias;
        Path = aliases[alias];
        aliases.Remove(alias);
        _aliasStorage.Save(aliases);
        return true;
    }

    public void Cancel()
    {
        if (Alias is null || Path is null)
        {
            return;
        }

        var aliases = _aliasStorage.Load<Dictionary<string, string>>();
        aliases.Add(Alias, Path);
        _aliasStorage.Save(aliases);
    }
}
