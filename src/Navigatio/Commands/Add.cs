using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Navigatio.Commands;

public class Add : IExecutable, ICancellable
{
    private readonly Aliases _storage;

    public Add(Aliases storage)
    {
        _storage = storage;
    }

    public string? Alias { get; set; }
    public string? OldPath { get; set; }

    public void Execute(params string[] args)
    {
        if (args.Length < 2 || args[0].StartsWith("-") || Regex.IsMatch(args[0], @"\/"))
        {
            throw new CommandUsageException();
        }

        string path = args[1];
        try
        {
            Directory.CreateDirectory(path);
        }
        catch
        {
            Console.WriteLine($"Path '{path}' is invalid.");
            return;
        }

        Dictionary<string, string> aliases = _storage.Load();
        string alias = args[0];

        if (!aliases.TryAdd(alias, path))
        {
            OldPath = aliases[alias];
            aliases[alias] = path;
        }

        Alias = alias;
        _storage.Save(aliases);
    }

    public void Cancel()
    {
        Debug.Assert(Alias is not null);

        Dictionary<string, string> aliases = _storage.Load();
        if (OldPath is null)
        {
            aliases.Remove(Alias);
        }
        else
        {
            aliases[Alias] = OldPath;
        }

        _storage.Save(aliases);
    }
}