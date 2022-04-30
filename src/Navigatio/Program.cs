using Navigatio;
using Navigatio.Storages;
using static System.IO.Path;

// TODO:
// do something with object and JObject in the history class
// do something with Load<Explicit type>
// add help

string exePath = AppContext.BaseDirectory;

var aliases = new JsonStorage(Join(exePath, "aliases.json"));
var historyStorage = new JsonStorage(Join(exePath, "history.json"));
var history = new History(historyStorage);
var commander = new Commander(aliases, history, Join(exePath, "output.sh"));

var app = new Application(args, commander, history);
app.Run();
