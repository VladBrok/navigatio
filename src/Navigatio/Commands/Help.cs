namespace Navigatio.Commands;

public class Help : IExecutable
{
    private readonly ICommander _commander;
    private readonly Table _table;

    public Help(ICommander commander, Table table)
    {
        _commander = commander;
        _table = table;
    }

    private string NewLine => $"{Environment.NewLine}      ";

    public bool Execute(params string[] args)
    {
        if (args.Length < 1)
        {
            foreach (CommandData c in _commander.GetAll())
            {
                ShowHelpFor(c);
            }
        }
        else
        {
            CommandData? c = _commander.Get(args[0]);
            if (c is null)
            {
                Console.WriteLine($"Command '{args[0]}' not found.");
                return false;
            }
            ShowHelpFor(c);
        }

        return true;
    }

    private void ShowHelpFor(CommandData c)
    {
        Console.WriteLine($"{NewLine}{c.Name}, {c.ShortName}");
        WriteWithMargin($"Description:{NewLine}{c.Description}");
        WriteWithMargin($"Usage:{NewLine}{c.Usage}");
        if (c.Arguments is null)
        {
            return;
        }

        WriteWithMargin($"Arguments:");
        _table.Print(
            c.Arguments.Select(x => x.Item1),
            c.Arguments.Select(x => x.Item2),
            columnGap: 4,
            marginLeft: 6);
    }

    private static void WriteWithMargin(string msg)
    {
        Console.WriteLine($"    {msg}");
    }
}
