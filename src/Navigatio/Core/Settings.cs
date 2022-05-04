namespace Navigatio;

public class Settings
{
    public int CommandHistoryLimit { get; set; }
    public string? FavoriteEditor { get; set; }

    public void ShallowCopyTo(Settings other)
    {
        other.CommandHistoryLimit = CommandHistoryLimit;
        other.FavoriteEditor = FavoriteEditor;
    }
}
