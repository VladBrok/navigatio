using System.Diagnostics;
using Navigatio.Storages;

namespace Navigatio.Commands;

public class ChangeSettings : IExecutable, ICancellable
{
    private readonly IStorage<Settings> _storage;

    public ChangeSettings(Settings current, IStorage<Settings> storage)
    {
        Settings = current;
        _storage = storage;
    }

    public Settings Settings { get; set; }

    public bool Execute(params string[] _)
    {
        string editor = Settings.FavoriteEditor ?? GetDefaultEditor();
        if (Settings.FavoriteEditor is null)
        {
            Console.WriteLine($"Favorite editor is not specified. Falling back to {editor}.");
        }

        Process? editorInstance;
        try
        {
            var info = new ProcessStartInfo(editor, _storage.File) { Verb = "Edit" };
            editorInstance = Process.Start(info) ?? throw new NullReferenceException();
        }
        catch
        {
            Console.WriteLine($"Failed to open your favorite editor ({editor}).");
            return false;
        }

        Console.WriteLine("Waiting for your editor to exit...");
        editorInstance?.WaitForExit();
        Console.WriteLine("Settings are changed.");
        return true;
    }

    public void Cancel()
    {
        _storage.Load(
            settings =>
            {
                Settings.ShallowCopyTo(settings);
                Console.WriteLine("Settings are changed.");
            }
        );
    }

    private string GetDefaultEditor()
    {
        return "notepad";
    }
}
