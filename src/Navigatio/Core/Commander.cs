using Navigatio.Commands;
using Navigatio.Storages;

namespace Navigatio;

public class Commander : ICommander
{
    private readonly Dictionary<string, CommandData> _commands;
    private readonly History _history;

    public Commander(
        IStorage<Dictionary<string, string>> aliases,
        History history,
        Table table,
        string shellFile,
        Settings settings,
        IStorage<Settings> settingsStorage)
    {
        _history = history;
        var data = new[]
        {
            new CommandData(
                "--add",
                "-a",
                () => new Add(aliases),
                "Adds a new alias for the specified path. If the same alias already exists, overwrites an old path.",
                "nav --add [alias] [path]",
                ("alias", "An alias for the path. It cannot start with '-' and cannot contain '\\' or '/'."),
                ("path", "A full, valid path to the folder. If a folder does not exist, it will be created. " +
                         "To take a current path simply write . (dot).")),
            new CommandData(
                "--del",
                "-d",
                () => new Delete(aliases),
                "Removes an alias if it exists.",
                "nav --del [alias]",
                ("alias", "An alias to delete.")),
            new CommandData(
                "--move",
                "-m",
                () => new Move(shellFile, aliases),
                "Performs cd to the path indicated by the alias.",
                "nav [alias]",
                ("alias", "An alias for the path.")),
            new CommandData(
                "--show",
                "-s",
                () => new Show(aliases, table),
                "Shows all aliases and the paths they point to.",
                "nav --show"),
            new CommandData(
                "--undo",
                "-u",
                () => new Undo(this, history),
                "Cancels the last command. Redo is not supported.",
                "nav --undo"),
            new CommandData(
                "--help",
                "-h",
                () => new Help(this, table),
                "Shows information about all commands.",
                "nav --help [command]",
                ("command", "Command to show information for. If omited, shows information for all commands.")),
            new CommandData(
                "--change-settings",
                "-cs",
                () => new ChangeSettings(settings, settingsStorage),
                "Opens a json file in the text editor where you can adjust some settings.",
                "nav --change-settings")
        };

        _commands = new Dictionary<string, CommandData>();
        InitializeCommands(data);
    }

    public void Run(string[] args)
    {
        if (!args.Any())
        {
            _commands["--help"].Executor().Execute();
            return;
        }

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

    public CommandData? Get(string name)
    {
        _commands.TryGetValue(name, out CommandData? result);
        return result;
    }

    public IEnumerable<CommandData> GetAll()
    {
        return _commands.Values.Distinct().AsEnumerable();
    }

    private void InitializeCommands(CommandData[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            _commands.Add(data[i].Name, data[i]);
            _commands.Add(data[i].ShortName, data[i]);
        }
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
