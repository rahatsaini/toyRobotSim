namespace ToyRobotSim
{
    public class Game
    {
        private const string Prompt = "Enter command (PLACE X,Y,FACING | MOVE | LEFT | RIGHT | REPORT):";

        private readonly Robot _robot = new();

        // Reads commands until the input ends. The input can be the keyboard (Console.In)
        // or a file, and the prompt is only shown when someone is typing.
        public void Start(TextReader input, bool showPrompt)
        {
            WritePrompt(showPrompt);
            while (input.ReadLine() is string command)
            {
                var output = Play(command);
                if (output != null)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(output);
                    Console.ResetColor();
                }
                WritePrompt(showPrompt);
            }
        }

        private static void WritePrompt(bool showPrompt)
        {
            if (showPrompt)
            {
                Console.Write(Prompt);
            }
        }

        public string? Play(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return null;
            }

            var parts = command.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (!TryParseName(parts[0], out Command commandName))
            {
                return null;
            }

            switch (commandName)
            {
                case Command.Place:
                    if (parts.Length == 2)
                    {
                        return HandlePlace(parts[1]);
                    }
                    break;
                case Command.Move:
                    if (parts.Length == 1)
                    {
                        return _robot.Move();
                    }
                    break;
                case Command.Left:
                    if (parts.Length == 1)
                    {
                        return _robot.Left();
                    }
                    break;
                case Command.Right:
                    if (parts.Length == 1)
                    {
                        return _robot.Right();
                    }
                    break;
                case Command.Report:
                    if (parts.Length == 1)
                    {
                        return _robot.Report();
                    }
                    break;
                // This was not the part of the requirements, but I added it to visualize the grid and the robot's position.
                case Command.Grid:
                    if (parts.Length == 1)
                    {
                        return GridRenderer.Render(_robot);
                    }
                    break;
            }
            return null;
        }

        private string? HandlePlace(string args)
        {
            var parts = args.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                return null;
            }

            var direction = parts[2];
            if (parts.Length == 3 &&
                int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y) &&
                Enum.GetNames<Direction>().Contains(direction, StringComparer.OrdinalIgnoreCase))
            {

                return _robot.Place(x, y, Enum.Parse<Direction>(direction, true));
            }
            return null;
        }

        // Case-insensitive match on the enum's names only.
        // Enum.TryParse would also accept numbers, e.g. "1" would become Direction.East.
        private static bool TryParseName<TEnum>(string value, out TEnum result) where TEnum : struct, Enum
        {
            foreach (var name in Enum.GetNames<TEnum>())
            {
                if (string.Equals(name, value, StringComparison.OrdinalIgnoreCase))
                {
                    result = Enum.Parse<TEnum>(name);
                    return true;
                }
            }
            result = default;
            return false;
        }
    }
}
