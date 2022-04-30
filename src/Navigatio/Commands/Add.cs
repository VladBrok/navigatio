using System.Diagnostics;
using System.Text.RegularExpressions;
using Navigatio.Storages;

namespace Navigatio.Commands;

public class Add : IExecutable, ICancellable
{
    private readonly IStorage<Dictionary<string, string>> _aliasStorage;

    public Add(IStorage<Dictionary<string, string>> aliasStorage)
    {
        _aliasStorage = aliasStorage;
    }

    public string? Alias { get; set; }
    public string? OldPath { get; set; }

    public bool Execute(params string[] args)
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
            return false;
        }

        string alias = args[0];
        _aliasStorage.Load(aliases =>
        {
            if (aliases.TryAdd(alias, path))
            {
                return;
            }
            OldPath = aliases[alias];
            aliases[alias] = path;
        });

        Alias = alias;
        return true;
    }

    public void Cancel()
    {
        Debug.Assert(Alias is not null);

        _aliasStorage.Load(aliases =>
        {
            if (OldPath is null)
            {
                aliases.Remove(Alias);
            }
            else
            {
                aliases[Alias] = OldPath;
            }
        });
    }
}
