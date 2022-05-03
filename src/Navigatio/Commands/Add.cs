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

        string path = args[1] == "."
                      ? Directory.GetCurrentDirectory().Replace('\\', '/')
                      : args[1];
        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Directory '{path}' created.");
            }
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
                Console.WriteLine($"Alias '{alias}' added.");
                return;
            }

            OldPath = aliases[alias];
            aliases[alias] = path;
            Console.WriteLine($"Alias '{alias}' updated. Previously pointed to '{OldPath}'.");
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
                Console.WriteLine($"Alias {Alias} deleted.");
                return;
            }

            aliases[Alias] = OldPath;
            Console.WriteLine($"Alias '{Alias}' updated. Now points to '{OldPath}'.");
        });
    }
}
