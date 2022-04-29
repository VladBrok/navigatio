using Navigatio;
using Navigatio.Storages;
using static System.IO.Path;

// TODO:
// refactor code duplication in commands
// add help


// var stack = new Stack<(string, int)>();
// stack.Push(("one", 5));
// stack.Push(("two", 2));
// stack.Push(("three", 9));
// string json = JsonConvert.SerializeObject(stack);
// System.Console.WriteLine(json);
// stack = new Stack<(string, int)>(JsonConvert.DeserializeObject<Stack<(string, int)>>(json));
// System.Console.WriteLine(JsonConvert.SerializeObject(stack));

string exePath = AppContext.BaseDirectory;

var aliases = new JsonStorage(Join(exePath, "aliases.json"));
var historyStorage = new JsonStorage(Join(exePath, "history.json"));
var history = new History(historyStorage);
var commander = new Commander(aliases, history, Join(exePath, "output.sh"));

var app = new Application(args, commander, history);
app.Run();
