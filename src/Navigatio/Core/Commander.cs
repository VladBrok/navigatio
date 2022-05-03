using Navigatio.Commands;
using Navigatio.Storages;

namespace Navigatio;

public class Commander
{
    private readonly Dictionary<string, CommandData> _commands;
    private readonly History _history;

    public Commander(
        IStorage<Dictionary<string, string>> aliases,
        History history,
        Table table,
        string shellOutputFile)
    {
        _history = history;
        var add = new CommandData(
            "--add",
            "-a",
            () => new Add(aliases),
            "Adds a new alias for the specified path. If the same alias already exists, overwrites an old path.",
            "nav --add [alias] [path]",
            ("alias", "An alias for the path. It cannot start with '-' and cannot contain '\\' or '/'."),
            ("path", "A full, valid path to the folder. If a folder does not exist, it will be created. " +
                     "To take a current path simply write . (dot)."));
        var delete = new CommandData(
            "--del",
            "-d",
            () => new Delete(aliases),
            "Removes an alias if it exists.",
            "nav --del [alias]",
            ("alias", "An alias to delete."));
        var move = new CommandData(
            "--move",
            "-m",
            () => new Move(shellOutputFile, aliases),
            "Performs cd to the path indicated by the alias.",
            "nav [alias]",
            ("alias", "An alias for the path."));
        var show = new CommandData(
            "--show",
            "-s",
            () => new Show(aliases, table),
            "Shows all aliases and the paths they point to.",
            "nav --show");
        var undo = new CommandData(
            "--undo",
            "-u",
            () => new Undo(this, history),
            "Cancels the last command. Redo is not supported.",
            "nav --undo");
        var help = new CommandData(
            "--help",
            "-h",
            () => new Help(this, table),
            "Shows information about all commands.",
            "nav --help [command]",
            ("command", "Command to show information for. If omited, shows information for all commands."));

        _commands = new Dictionary<string, CommandData>();
        foreach (CommandData c in new[] { add, delete, move, show, undo, help })
        {
            _commands.Add(c.Name, c);
            _commands.Add(c.ShortName, c);
        }
    }

    public void Run(string[] args)
    {
        (string name, string[] arguments) = ExtractCommandInfo(args);
        try
        {
            ExecuteCommand(name, arguments);
        }
        catch (CommandUsageException)
        {
            Console.WriteLine("Invalid usage of the command.");
            _commands["--help"].Executor().Execute(name);
        }
    }

    public IExecutable Get(string name) // TODO: Remove ?
    {
        return _commands[name].Executor();
    }

    public IEnumerable<CommandData> GetAllCommands()
    {
        return _commands.Values.Distinct().AsEnumerable();
    }

    public CommandData? GetCommand(string name)
    {
        _commands.TryGetValue(name, out CommandData? result);
        return result;
    }

    private (string name, string[] arguments) ExtractCommandInfo(string[] args)
    {
        return _commands.ContainsKey(args[0])
               ? (args[0], args[1..])
               : ("--move", args);
    }

    private void ExecuteCommand(string name, string[] arguments)
    {
        IExecutable command = _commands[name].Executor();
        bool executed = command.Execute(arguments);
        if (executed && command is ICancellable c)
        {
            _history.Push(name, c);
        }
    }
}
