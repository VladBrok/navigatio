namespace Navigatio.Commands;

public class Show : IExecutable
{
    private readonly Aliases _storage;

    public Show(Aliases storage)
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
