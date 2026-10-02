# Toy Robot Simulator

A C# (.NET 10) console app that simulates a toy robot moving on a 5 x 5 table.

## Build, test and run

```bash
dotnet build src.slnx
dotnet test src.slnx
dotnet run --project src                                 # type commands, Ctrl+Z / Ctrl+D to finish
dotnet run --project src -- test-data/example-3.txt      # read commands from a file
```

With a file, the app runs every command, prints the output and exits. A missing or unreadable file (for example, locked by another program) prints an error and exits with code 1. In Visual Studio, set the file path (in full) under **Project Properties > Debug > launch profiles**.

```
PLACE 1,2,EAST
MOVE
MOVE
LEFT
MOVE
REPORT
Robot is at [3,3] facing NORTH
```

## Commands

| Command | Description |
|---|---|
| `PLACE X,Y,F` | Puts the robot at X,Y facing NORTH, SOUTH, EAST or WEST. (0,0) is the south-west corner |
| `MOVE` | Moves one square forward |
| `LEFT` / `RIGHT` | Turns 90 degrees without moving |
| `REPORT` | Prints `Robot is at [X,Y] facing DIRECTION` |
| `GRID` | *Hidden extra, not in the brief:* draws the table and the robot |

`GRID` after the example above (arrows `^ > v <` show the way the robot faces, and north is at the top):

```
           N
  4 [ ][ ][ ][ ][ ]
  3 [ ][ ][^][ ][ ]
W 2 [ ][ ][ ][ ][ ] E
  1 [ ][ ][ ][ ][ ]
  0 [ ][ ][ ][ ][ ]
     0  1  2  3  4
           S
```

## Rules and assumptions

- **The robot can never leave the table.** A `MOVE` off an edge is silently ignored, and a `PLACE` off the table is ignored and prints `Invalid placement`. The robot stays where it was, and later commands still work.
- **Placement first.** `MOVE`, `LEFT`, `RIGHT` and `REPORT` before the first valid `PLACE` are ignored and print `Robot is not placed`.
- **Forgiving format, strict values.** Commands are case-insensitive, and `PLACE` values can be separated by commas and/or spaces, with leading zeros (`place 01,2 east` works). Anything else that can't be understood is ignored without crashing: `PLACE 1,2,1` (directions must be names), `PLACE 1,2,UP`, `PLACE a,b,NORTH`, `MOVE xxx`, unknown commands and blank lines. `PLACE +1,2,EAST` and `PLACE 1,,2,EAST` are also accepted as 1,2.

## Scalability

The table is 5 x 5 by default, but the size is a constructor parameter on `Robot`, not hard-coded. Changing it is one line in `src/Game.cs`: `new()` becomes `new(10)`. The bounds checks and `GRID` drawing follow automatically. Limits: the size is set in code rather than from a command-line argument, and `GRID` only lines up up to 10 x 10, because two-digit coordinates are wider.

## Project structure

```
src/
  Program.cs            entry point: reads from a file if given, otherwise the console
  Game.cs               turns each line of input into a robot command
  Robot.cs              position, direction and the movement rules
  Direction.cs          Direction enum and TurnLeft/TurnRight
  Command.cs            Command enum
  GridRenderer.cs       draws the table for GRID
ToyRobotSim.Test/       xUnit tests: RobotTests, DirectionTests, GameTests, GridRendererTests
test-data/              command files (*.txt) with expected output (*.expected)
```

## Tests and test data

- **Unit tests** cover every class with logic and run with `dotnet test`. They're named **`MethodUnderTest_Scenario_ExpectedBehavior`** (e.g. `Move_OffEdge_DoesNotChangePosition`). In `GameTests` the first part is the command being sent, `Play` for input handling and `Commands` for tests that cover several commands.
- **Test data** in `test-data/` exercises the app end to end: the three examples from the brief, commands before PLACE, every edge, a walk around the perimeter, and invalid input. Run one with `dotnet run --project src -- test-data/edges.txt` and compare the output with the matching `.expected` file.

## Development process

1. **Understand the problem.** I picked out the key rules (never fall off, ignore everything before a valid PLACE, keep going after an ignored command) and worked the brief's examples by hand.
2. **Evaluate possible solutions.** I chose a small design: `Robot` holds the rules and `Game` turns text into commands, so the rules can be tested without the console. I also compared against a reference solution (see [Use of AI](#use-of-ai)).
3. **Build the MVP.** The five commands working from the console, checked against the brief's examples.
4. **Add tests.** Unit tests for each class. Code reviews found bugs, such as `PLACE 1,2,1` being accepted as EAST and `MOVE 5` still moving the robot. I fixed them and added tests so they can't come back.
5. **Refactor.** With the tests in place: turning moved to extension methods, one type per file, PascalCase enums, read-only robot state, constants for repeated strings, narrower exception handling, and an input loop that can read from a file.

### Trade-offs

- **Turning doesn't depend on enum order.** Arithmetic like `(Direction)(((int)Facing + 1) % 4)` is shorter and scales to 8 directions, but silently breaks if the enum is reordered. Explicit `switch` mappings are longer but safe and easy to read, which matters more with four fixed directions.
- **`Left()` and `Right()` rather than one `ChangeDirection(bool)`.** They match the brief's commands and say which way the robot turns.
- **Directions matched by name.** `Enum.TryParse` would accept `"1"` as EAST, so input is checked against `Enum.GetNames` instead.
- **Messages for ignored commands.** Clearer for someone typing, though the brief allows silence. A blocked `MOVE` stays silent, because it's expected rather than a mistake.
- **`Robot` returns message text.** Simpler, but it mixes rules with wording. Returning `bool` and letting `Game` choose the text would separate them better.
- **GRID is separate from REPORT.** REPORT's output is defined by the brief, each command does one thing, and a large grid (100 x 100 is about 300 characters wide) wouldn't fit on screen. I first worried that tests comparing large grids would be slow, but string comparison is cheap and small grids prove the logic. If wanted, an opt-in `--show-grid` option would be the clean way to combine them.
- **GRID in its own `GridRenderer` class**, so `Robot` has no display code and the drawing can be unit tested.

## Use of AI

I used **Claude** (Anthropic), through the Claude Code desktop app, as an assistant throughout this project. The full conversation is attached with this submission. The prompts below are quoted from it as I typed them.

### What AI was used for

| Area | What the AI did | What I did |
|---|---|---|
| Getting started | Generated a reference solution in C#, then a simpler version of it | Wrote this project myself, using the reference for comparison |
| Code reviews | Reviewed my code for bugs, C# conventions and overlapping tests | Decided which findings to act on. Fixed some myself (input validation, private setters, the enum rename, test expectations) and asked the AI to apply others |
| Unit tests | Suggested test cases for `Game` | Added them to the project, removed duplicates and resolved conflicting expectations |
| GRID command | Wrote `GridRenderer`, its tests and the compass labels | Proposed the feature, chose the layout (north at the top, N/S/E/W labels) |
| Refactoring | Updated the code after I renamed the enums to PascalCase, applied C# convention fixes, and added reading commands from a file | Chose what to change. I kept my own `HandlePlace` logic where I preferred it |
| Test data | Wrote the `test-data` files and expected outputs | Decided what test data meant for the brief, and had automated test-data tests removed as unnecessary |
| README | Drafted this README from my outline and direction | Set the structure, the development process steps and the trade-offs to cover |

### Prompts used


> You are an expert software engineer specialising in scalable development and writing robust, maintainable unit tests.

> Generate xUnit test cases for `Game.cs` that cover the happy path, negative paths and boundary cases. Each test should check one behaviour, and use `[Theory]` where only the input changes.

> Add a `GRID` command that draws the table and the robot's current position and direction. Keep the drawing in its own class so `Robot` has no display code, and add unit tests for the layout.

> Write a README covering how to build, run and test the app, the assumptions made, the design decisions and the trade-offs.

> Review the code for bugs, C# and .NET conventions, and duplicated tests. List the findings by severity with file and line, and don't change any code.
