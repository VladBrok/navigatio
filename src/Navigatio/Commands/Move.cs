using System.Diagnostics;
using Navigatio.Storages;

namespace Navigatio.Commands;

public class Move : IExecutable, ICancellable
{
    private readonly string _outputFile;
    private readonly IStorage<Dictionary<string, string>> _aliasStorage;

    public Move(string outputFile, IStorage<Dictionary<string, string>> aliasStorage)
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
        string? subfolder = ExtractSubfolder(ref alias);
        string? path = null;

        _aliasStorage.Load(aliases =>
        {
            aliases.TryGetValue(alias, out path);
        });

        if (path is null)
        {
            Console.WriteLine($"Alias '{alias}' not found.");
            return false;
        }

        MoveToPath(subfolder is null ? path : Path.Join(path, subfolder));
        return true;
    }

    private void MoveToPath(string path)
    {
        using var writer = new StreamWriter(_outputFile);
        writer.WriteLine("#!/usr/bin/env");
        writer.WriteLine($"cd {path}");
    }

    private static string? ExtractSubfolder(ref string alias)
    {
        string? subfolder = null;
        int slash = alias.IndexOf("/");
        if (slash != -1)
        {
            subfolder = alias[slash..];
            alias = alias[..slash];
        }

        return subfolder;
    }
}
