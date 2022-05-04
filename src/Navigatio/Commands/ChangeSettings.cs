using System.Diagnostics;
using Navigatio.Storages;

namespace Navigatio.Commands;

public class ChangeSettings : IExecutable, ICancellable
{
    private readonly string _settingsFile;
    private readonly IStorage<Settings> _storage;

    public ChangeSettings(Settings current, string settingsFile, IStorage<Settings> storage)
    {
        OldSettings = current;
        _settingsFile = settingsFile;
        _storage = storage;
    }

    public Settings OldSettings { get; set; }

    public bool Execute(params string[] _)
    {
        Process? editor;
        try
        {
            var info = new ProcessStartInfo(OldSettings.FavoriteEditor ?? "", _settingsFile)
            {
                Verb = "Edit"
            };
            editor = Process.Start(info) ?? throw new NullReferenceException();
        }
        catch
        {
            Console.WriteLine($"Failed to open your favorite editor ({OldSettings.FavoriteEditor}).");
            return false;
        }

        Console.WriteLine("Waiting for your editor to exit...");
        editor?.WaitForExit();
        Console.WriteLine("Settings are changed.");
        return true;
    }

    public void Cancel()
    {
        _storage.Load(settings =>
        {
            OldSettings.ShallowCopyTo(settings);
            Console.WriteLine("Settings are changed.");
        });
    }
}
