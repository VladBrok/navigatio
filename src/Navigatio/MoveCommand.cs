using System.Diagnostics;

public class MoveCommand : IExecutable, ICancellable
{
    private readonly string _outputFile;
    private readonly AliasesStorage _storage;

    public MoveCommand(string outputFile, AliasesStorage storage)
    {
        _outputFile = outputFile;
        _storage = storage;
    }

    public string? OldAlias { get; set; }

    public void Execute(params string[] args)
    {
        if (args.Length < 1)
        {
            throw new CommandUsageException();
        }

        string alias = args[0];
        MoveTo(alias);
        OldAlias = alias;
    }

    public void Cancel()
    {
        Debug.Assert(OldAlias is not null);

        MoveTo(OldAlias);
    }

    private void MoveTo(string alias)
    {
        if (!_storage.Load().TryGetValue(alias, out string? path))
        {
            Console.WriteLine($"Alias '{alias}' not found.");
            return;
        }

        using var writer = new StreamWriter(_outputFile);
        writer.WriteLine($"cd {path}");
    }
}
