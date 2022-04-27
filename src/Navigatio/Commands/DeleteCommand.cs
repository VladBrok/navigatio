namespace Navigatio.Commands;

public class DeleteCommand : IExecutable, ICancellable
{
    private readonly AliasesStorage _storage;

    public DeleteCommand(AliasesStorage storage)
    {
        _storage = storage;
    }

    public string? Alias { get; set; }
    public string? Path { get; set; }

    public void Execute(params string[] args)
    {
        if (args.Length < 1)
        {
            throw new CommandUsageException();
        }

        string alias = args[0];
        Dictionary<string, string> aliases = _storage.Load();
        if (!aliases.ContainsKey(alias))
        {
            return;
        }

        Alias = alias;
        Path = aliases[alias];
        aliases.Remove(alias);
        _storage.Save(aliases);
    }

    public void Cancel()
    {
        if (Alias is null || Path is null)
        {
            return;
        }

        Dictionary<string, string> aliases = _storage.Load();
        aliases.Add(Alias, Path);
        _storage.Save(aliases);
    }
}
