namespace ToyRobotSim
{
    public class Game
    {
        private readonly Robot _robot = new();


        public void Start()
        {
            string? input;
            try
            {
                while (true)
                {
                    Console.Write("Enter command (PLACE X,Y,FACING | MOVE | LEFT | RIGHT | REPORT): ");
                    input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        continue;
                    }
                    var output = Play(input);
                    if (output != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(output);
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        public string? Play(string command)
        {
            if (string.IsNullOrEmpty(command?.Trim()))
            {
                return null;
            }

            var parts = command.Trim().ToUpper().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            var commandName = parts[0];
            switch (commandName)
            {
                case nameof(Command.PLACE):
                    if (parts.Length == 2)
                    {
                        return HandlePlace(parts[1]);
                    }
                    break;
                case nameof(Command.MOVE):
                    if (parts.Length == 1)
                    {
                        return _robot.Move();
                    }
                    break;
                case nameof(Command.LEFT):
                    if (parts.Length == 1)
                    {
                        _robot.Left();
                    }
                    break;
                case nameof(Command.RIGHT):
                    if (parts.Length == 1)
                    {
                        _robot.Right();
                    }
                    break;
                case nameof(Command.REPORT):
                    return _robot.Report();
            }
            return null;
        }

        private string? HandlePlace(string args)
        {
            var parts = args.Split(',').Select(p => p.Trim()).ToArray();
            if (parts.Length == 3 &&
                int.TryParse(parts[0], out int x) &&
                int.TryParse(parts[1], out int y) &&
                Enum.TryParse<Direction>(parts[2], true, out var facing))
            {
                return _robot.Place(x, y, facing);
            }
            return null;
        }
    }
}
