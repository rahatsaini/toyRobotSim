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

var game = new Game();

if (args.Length == 1)
{
    // The File.Exists check above gives a friendly message for the common case, but the file
    // can still fail to open or read: deleted after the check, locked by another process,
    // or no permission to read it.
    // This is an AI suggesion to handle those cases gracefully.
    try
    {
        using var file = new StreamReader(args[0]);
        game.Start(file, showPrompt: false);
    }
    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
    {
        WriteError($"Could not read file '{args[0]}': {ex.Message}");
        Environment.ExitCode = 1;
    }
}
else
{
    // No prompt when commands are piped in, e.g. Get-Content commands.txt | dotnet run
    game.Start(Console.In, showPrompt: !Console.IsInputRedirected);
}

static void WriteError(string message)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.Error.WriteLine(message);
    Console.ResetColor();
}
