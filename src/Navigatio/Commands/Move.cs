using System.Diagnostics;

namespace Navigatio.Commands;

public class Move : IExecutable, ICancellable
{
    private readonly string _outputFile;
    private readonly Aliases _storage;

    public Move(string outputFile, Aliases storage)
    {
        _outputFile = outputFile;
        _storage = storage;
    }

    public string? OldPath { get; set; }

    public void Execute(params string[] args)
    {
        if (args.Length < 1)
        {
            throw new CommandUsageException();
        }

        string alias = args[0];
        OldPath = Directory.GetCurrentDirectory().Replace('\\', '/');
        MoveToAlias(alias);
    }

    public void Cancel()
    {
        Debug.Assert(OldPath is not null);

        MoveToPath(OldPath);
    }

    private void MoveToAlias(string alias)
    {
        string? subFolder = null;
        int slash = alias.IndexOf("/");
        if (slash != -1)
        {
            subFolder = alias[slash..];
            alias = alias[..slash];
        }

        if (!_storage.Load().TryGetValue(alias, out string? path))
        {
            Console.WriteLine($"Alias '{alias}' not found.");
            return;
        }

        MoveToPath(subFolder is null ? path : Path.Join(path, subFolder));
    }

    private void MoveToPath(string path)
    {
        using var writer = new StreamWriter(_outputFile);
        writer.WriteLine("#!/usr/bin/env");
        writer.WriteLine($"cd {path}");
    }
}
