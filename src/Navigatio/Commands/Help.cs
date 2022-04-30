namespace Navigatio.Commands;

public class Help : IExecutable
{
    private readonly Commander _commander;

    public Help(Commander commander)
    {
        _commander = commander;
    }

    private string NewLine => $"{Environment.NewLine}      ";

    public bool Execute(params string[] args)
    {
        if (args.Length < 1)
        {
            foreach (Command c in _commander.GetAllCommands())
            {
                ShowHelpFor(c);
            }
        }
        else
        {
            Command? c = _commander.GetCommand(args[0]);
            if (c is null)
            {
                Console.WriteLine($"Command '{args[0]}' not found.");
                return false;
            }
            ShowHelpFor(c);
        }

        return true;
    }

    private void ShowHelpFor(Command c)
    {
        Console.WriteLine();
        Console.WriteLine($"{c.Name}, {c.ShortName}");
        WriteWithMargin($"Description:{NewLine}{c.Description}");
        WriteWithMargin($"Usage:{NewLine}{c.Usage}");
        if (c.Arguments is null)
        {
            return;
        }

        WriteWithMargin($"Arguments:");
        int maxWidth = c.Arguments.Max(x => x.Item1.Length) + 2;
        foreach (var arg in c.Arguments)
        {
            WriteWithMargin($"{arg.Item1.PadLeft(maxWidth)}  {arg.Item2}");
        }
    }

    private void WriteWithMargin(string msg)
    {
        Console.WriteLine($"    {msg}");
    }
}
