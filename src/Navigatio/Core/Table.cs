namespace Navigatio;

public class Table
{
    public void Print(
        IEnumerable<string> firstColumn,
        IEnumerable<string> secondColumn,
        int columnGap = 2,
        int marginLeft = 2
    )
    {
        int maxWidth = firstColumn.Max(x => x.Length) + marginLeft;
        var gap = new string(' ', columnGap);

        foreach (var (First, Second) in firstColumn.Zip(secondColumn))
        {
            Console.WriteLine($"{First.PadLeft(maxWidth)}{gap}{Second}");
        }
    }
}
