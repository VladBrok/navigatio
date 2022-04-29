using System.Diagnostics;
using Navigatio.Storages;

namespace Navigatio.Commands;

public class Move : IExecutable, ICancellable
{
    private readonly string _outputFile;
    private readonly IStorage _aliasStorage;

    public Move(string outputFile, IStorage aliasStorage)
    {
        _outputFile = outputFile;
        _aliasStorage = aliasStorage;
    }

    public string? OldPath { get; set; }

    public bool Execute(params string[] args)
    {
        if (args.Length < 1)
        {
            throw new CommandUsageException();
        }

        string alias = args[0];
        OldPath = Directory.GetCurrentDirectory().Replace('\\', '/');
        return MoveToAlias(alias);
    }

    public void Cancel()
    {
        Debug.Assert(OldPath is not null);

        MoveToPath(OldPath);
    }

    private bool MoveToAlias(string alias)
    {
        string? subFolder = null;
        int slash = alias.IndexOf("/");
        if (slash != -1)
        {
            subFolder = alias[slash..];
            alias = alias[..slash];
        }

        if (!_aliasStorage.Load<Dictionary<string, string>>().TryGetValue(alias, out string? path))
        {
            Console.WriteLine($"Alias '{alias}' not found.");
            return false;
        }

        MoveToPath(subFolder is null ? path : Path.Join(path, subFolder));
        return true;
    }

    private void MoveToPath(string path)
    {
        using var writer = new StreamWriter(_outputFile);
        writer.WriteLine("#!/usr/bin/env");
        writer.WriteLine($"cd {path}");
    }
}
