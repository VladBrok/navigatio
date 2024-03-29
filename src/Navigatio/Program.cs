﻿using Navigatio;
using Navigatio.Storages;
using static System.IO.Path;

string exePath = AppContext.BaseDirectory;
string settingsFile = Join(exePath, "settings.json");

var settingsStorage = new JsonStorage<Settings>(settingsFile);
var aliases = new JsonStorage<Dictionary<string, string>>(Join(exePath, "aliases.json"));
var historyStorage = new JsonStorage<LinkedList<(string, object)>>(Join(exePath, "history.json"));

Settings? settings = null;
settingsStorage.Load(
    s =>
    {
        settings = s;
    },
    modifiesData: false
);
var history = new History(historyStorage, settings!.CommandHistoryLimit);
var table = new Table();

var commander = new Commander(
    aliases,
    history,
    table,
    shellFile: args[0],
    settings,
    settingsStorage
);
commander.Run(args[1..]);
