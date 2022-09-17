using System.Diagnostics;
using Navigatio.Storages;
using System.Runtime.InteropServices;

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
        string editor = "";
        try
        {
            editor = Settings.FavoriteEditor ?? GetDefaultEditor();
        }
        catch (PlatformNotSupportedException)
        {
            Console.WriteLine(
                $"Sorry, your OS is currently not supported. Please open file {_storage.File} and edit it manually."
            );
            return false;
        }

        if (Settings.FavoriteEditor is null)
        {
            Console.WriteLine($"Note: favorite editor is not specified. Falling back to {editor}.");
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
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "vi";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "notepad";
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
