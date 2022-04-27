namespace Navigatio.Commands;

public class ShowCommand : IExecutable
{
    private readonly AliasesStorage _storage;

    public ShowCommand(AliasesStorage storage)
    {
        _storage = storage;
    }

    public void Execute(params string[] _)
    {
        Console.WriteLine();
        foreach (var alias in _storage.Load())
        {
            Console.WriteLine($"  {alias.Key}  ->  {alias.Value}");
        }
    }
}
