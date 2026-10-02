using ToyRobotSim;

// Usage:
//   dotnet run --project src                        type commands in
//   dotnet run --project src -- <commands-file>     read commands from a file

if (args.Length > 1)
{
    WriteError("Usage: ToyRobotSim [commands-file]");
    Environment.ExitCode = 2;
    return;
}

if (args.Length == 1 && !File.Exists(args[0]))
{
    WriteError($"File not found: {args[0]}");
    Environment.ExitCode = 1;
    return;
}

try
{
    var game = new Game();

    if (args.Length == 1)
    {
        using var file = new StreamReader(args[0]);
        game.Start(file, showPrompt: false);
    }
    else
    {
        // No prompt when commands are piped in, e.g. Get-Content commands.txt | dotnet run
        game.Start(Console.In, showPrompt: !Console.IsInputRedirected);
    }
}
catch (ArgumentOutOfRangeException ex)
{
    WriteError($"An error occurred: {ex.Message}");
    Environment.ExitCode = 1;
}

static void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(message);
    Console.ResetColor();
}
