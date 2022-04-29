using Navigatio;
using static System.IO.Path;

// TODO:
// message 'nothing to undo' is in the History ????
// fix history saving when command didn't complete
// find out WHY System json DOES NOT SERIALIZE tuples
// add help
// refactor code duplication (aliases, history, aliases & history)


// var stack = new Stack<(string, int)>();
// stack.Push(("one", 5));
// stack.Push(("two", 2));
// stack.Push(("three", 9));
// string json = JsonConvert.SerializeObject(stack);
// System.Console.WriteLine(json);
// stack = new Stack<(string, int)>(JsonConvert.DeserializeObject<Stack<(string, int)>>(json));
// System.Console.WriteLine(JsonConvert.SerializeObject(stack));

string exePath = AppContext.BaseDirectory;

var aliases = new Aliases(Join(exePath, "aliases.json"));
var history = new History(Join(exePath, "history.json"));
var commander = new Commander(aliases, history, Join(exePath, "output.sh"));

var app = new Application(args, commander, history);
app.Run();
