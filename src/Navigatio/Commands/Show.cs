namespace Navigatio.Commands;

public class Show : IExecutable
{
    private readonly AliasesStorage _storage;

    public Show(AliasesStorage storage)
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
