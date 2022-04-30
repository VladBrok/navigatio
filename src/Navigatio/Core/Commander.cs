using Navigatio.Commands;
using Navigatio.Storages;

namespace Navigatio;

public class Commander
{
    private readonly Dictionary<string, Command> _commands;

    public Commander(
        IStorage<Dictionary<string, string>> aliases,
        History history,
        string shellOutputFile)
    {
        var add = new Command(
            "--add",
            "-a",
            () => new Add(aliases),
            "Adds a new alias for the specified path. If the same alias already exists, overwrites an old path.",
            "nav --add [alias] [path]",
            ("alias", "An alias for the path."),
            ("path", "A full, valid path to the folder. If a folder does not exist it will be created. " +
                     "To take a current path simply write . (dot)."));
        var delete = new Command(
            "--del",
            "-d",
            () => new Delete(aliases),
            "Removes an alias if it exists.",
            "nav --del [alias]",
            ("alias", "An alias to delete."));
        var move = new Command(
            "--move",
            "-m",
            () => new Move(shellOutputFile, aliases),
            "Performs cd to the path indicated by the alias. You can just type 'nav [alias]'",
            "nav [alias]",
            ("alias", "An alias for the path."));
        var show = new Command(
            "--show",
            "-s",
            () => new Show(aliases),
            "Shows all aliases and the paths they point to.",
            "nav --show");
        var undo = new Command(
            "--undo",
            "-u",
            () => new Undo(this, history),
            "Cancels the last command. You cannot undo it.",
            "nav --undo");
        var help = new Command(
            "--help",
            "-h",
            () => new Help(this),
            "Shows information about all commands.",
            "nav --help [command]",
            ("command", "Command to show information for. If omited, shows information for all commands."));

        _commands = new Dictionary<string, Command>();
        foreach (Command c in new[] { add, delete, move, show, undo, help })
        {
            _commands.Add(c.Name, c);
            _commands.Add(c.ShortName, c);
        }
    }

    public IExecutable Get(string name)
    {
        return _commands[name].Executor();
    }

    public IExecutable Get(ref string[] args)
    {
        if (_commands.TryGetValue(args[0], out Command? c))
        {
            return c.Executor();
        }

        args = args.Prepend("--move").ToArray();
        return _commands["--move"].Executor();
    }

    public IEnumerable<Command> GetAllCommands()
    {
        return _commands.Values.Distinct().AsEnumerable();
    }

    public Command? GetCommand(string name)
    {
        _commands.TryGetValue(name, out Command? result);
        return result;
    }
}
