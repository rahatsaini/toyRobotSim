namespace ToyRobotSim
{
    public class Robot
    {
        private const string NotPlacedMessage = "Robot is not placed";

        private readonly int _gridSize;

        public int GridSize => _gridSize;

        public int X { get; private set; }
        public int Y { get; private set; }

        public Direction Facing { get; private set; }

        public bool IsPlaced { get; private set; }

        public Robot(int gridSize = 5)
        {
            if (gridSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(gridSize), "Grid size must be positive");
            }
            _gridSize = gridSize;
        }

        public string? Place(int x, int y, Direction facing)
        {
            // a value like (Direction)99. Game never passes one, so this is a programming error.
            // this is an AI suggestion to make the code more robust, but it is not strictly necessary for the brief.
            if (!Enum.IsDefined(facing))
            {
                throw new ArgumentOutOfRangeException(nameof(facing), facing, "Facing must be a defined direction");
            }

            if (!IsInBounds(x, y))
            {
                return "Invalid placement";
            }
            X = x;
            Y = y;
            Facing = facing;
            IsPlaced = true;
            return null;
        }

        public string? Move()
        {
            if (!IsPlaced)
            {
                return NotPlacedMessage;
            }
            int newX = X;
            int newY = Y;
            switch (Facing)
            {
                case Direction.North:
                    {
                        newY++;
                        break;
                    }
                case Direction.East:
                    {
                        newX++;
                        break;
                    }
                case Direction.South:
                    {
                        newY--;
                        break;
                    }
                case Direction.West:
                    {
                        newX--;
                        break;
                    }
                default:
                    throw new InvalidOperationException($"Unknown direction {Facing}");
            }
            if (IsInBounds(newX, newY))
            {
                X = newX;
                Y = newY;
            }
            return null;
        }

        public string? Left()
        {
            if (!IsPlaced)
            {
                return NotPlacedMessage;
            }
            Facing = Facing.TurnLeft();
            return null;
        }

        public string? Right()
        {
            if (!IsPlaced)
            {
                return NotPlacedMessage;
            }
            Facing = Facing.TurnRight();
            return null;
        }


        public string? Report()
        {
            if (!IsPlaced)
            {
                return NotPlacedMessage;
            }
            // Enum names are PascalCase, but the brief's output uses upper case (NORTH)
            return $"Robot is at [{X},{Y}] facing {Facing.ToString().ToUpperInvariant()}";
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _gridSize && y < _gridSize;
        }
    }
}
