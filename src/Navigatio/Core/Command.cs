using Navigatio.Commands;

namespace Navigatio;

public class Command
{
    public Command(
        string name,
        string shortName,
        Func<IExecutable> executor,
        string description,
        string usage,
        params (string, string)[] arguments)
    {
        Name = name;
        ShortName = shortName;
        Executor = executor;
        Description = description;
        Usage = usage;
        Arguments = arguments.Any() ? arguments : null;
    }

    public string Name { get; init; }
    public string ShortName { get; init; }
    public Func<IExecutable> Executor { get; init; }
    public string Description { get; init; }
    public string Usage { get; init; }
    public IEnumerable<(string, string)>? Arguments { get; init; }
}
