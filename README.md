<div align="center">

# Navigatio

![video demonstration](assets/videos/demo.gif)

Fast navigation between directories

</div>

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Installation](#installation)
  - [Linux](#linux)
  - [Windows](#windows)
- [Usage](#usage)
- [License](#license)

## Introduction

If you use a **terminal** and you need to move between directories frequently, then this application is for you! Define aliases and use them to **quickly navigate between directories**. \
If you accidentally used the wrong command, don't worry. The cancellation command `nav -u` will take care of this!

## Features

- **Create aliases** for directories and **navigate to them quickly**
- **Cancel** previous actions

## Installation

### Linux

1. Download and unpack the latest [release](https://github.com/VladBrok/navigatio/releases) for linux (file linux-x64.zip)

2. Open the folder with unpacked files in the terminal and run the setup

```bash
sudo chmod +x setup.sh && ./setup.sh ./
```

3. Restart the terminal

Done! Now you can run `nav -h` for a list of all available commands

### Windows

1. Download and install [git](https://git-scm.com/download/win) (you'll need a git bash in order to use the app)

2. Download and unpack the latest [release](https://github.com/VladBrok/navigatio/releases) for your specific architecture (win-x64.zip or win-x86.zip)

3. Open the folder with unpacked files in the git bash and run the setup

```bash
./setup.sh ./
```

4. Restart the git bash

Done! Now you can run `nav -h` for a list of all available commands

## Usage

Navigation:

- `nav <alias>` Navigate to this alias

Managing aliases:

- `nav --help|-h` A list of all commands
- `nav --add|-a <alias> <relative_or_absolute_path>` Add an alias
- `nav --delete|-d <alias>` Remove an alias
- `nav --show|-s` List all of your aliases

Cancelling actions:

- `nav --undo|-u` Undo the previous action

Changing settings:

- `nav --change-settings|-cs` Adjust a limit of the command history and set your favorite editor

## License

Navigatio is available under the [MIT license](https://opensource.org/licenses/MIT)
