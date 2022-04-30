using Navigatio;
using Navigatio.Storages;
using Newtonsoft.Json;
using static System.IO.Path;

// TODO:
// do something with object and JObject in the history class
// do something with Load<Explicit type>
// add help


// var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
// var stack = new LinkedList<(string?, int)>();
// stack.AddFirst(("one", 5));
// stack.AddFirst(("two", 2));
// stack.AddFirst(("three", 9));
// stack.AddFirst((null, 9));
// string json = JsonConvert.SerializeObject(stack, settings);
// System.Console.WriteLine(json);
// stack = JsonConvert.DeserializeObject<LinkedList<(string, int)>>(json, settings);
// System.Console.WriteLine(JsonConvert.SerializeObject(stack));


string exePath = AppContext.BaseDirectory;

var aliases = new JsonStorage(Join(exePath, "aliases.json"));
var historyStorage = new JsonStorage(Join(exePath, "history.json"));
var history = new History(historyStorage);
var commander = new Commander(aliases, history, Join(exePath, "output.sh"));

var app = new Application(args, commander, history);
app.Run();
