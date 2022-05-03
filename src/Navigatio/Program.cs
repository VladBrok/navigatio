using System.Diagnostics;
using Navigatio;
using Navigatio.Storages;
using static System.IO.Path;

// TODO:
// add windows cmd support
// optimize file IO
// make history limit ?

string exePath = AppContext.BaseDirectory;
var aliases = new JsonStorage<Dictionary<string, string>>(
    Join(exePath, "aliases.json"));
var historyStorage = new JsonStorage<LinkedList<(string, object)>>(
    Join(exePath, "history.json"));

var history = new History(historyStorage);
var table = new Table();

var commander = new Commander(aliases, history, table, shellFile: args[0]);

var watch = Stopwatch.StartNew();
commander.Run(args[1..]);

// Average execution time: 1000 ms
// Looks like the History is a bottleneck
Console.WriteLine($"Executed in {watch.ElapsedMilliseconds} ms.");
