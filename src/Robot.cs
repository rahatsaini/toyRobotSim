namespace ToyRobotSim
{
    public class Robot
    {
        private readonly int _gridSize;

        public int X { get; set; }
        public int Y { get; set; }

        public Direction Facing { get; set; }

        public bool IsPlaced { get; set; }
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
                return "Robot is not placed";
            }
            int newX = X;
            int newY = Y;
            switch (Facing)
            {
                case Direction.NORTH:
                    {
                        newY++;
                        break;
                    }
                case Direction.EAST:
                    {
                        newX++;
                        break;
                    }
                case Direction.SOUTH:
                    {
                        newY--;
                        break;
                    }
                case Direction.WEST:
                    {
                        newX--;
                        break;
                    }

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
                return "Robot is not placed";
            }
            Facing = Facing.TurnLeft();
            return null;
        }

        public string? Right()
        {
            if (!IsPlaced)
            {
                return "Robot is not placed";
            }
            Facing = Facing.TurnRight();
            return null;
        }


        public string? Report()
        {
            if (!IsPlaced)
            {
                return "Robot is not placed"; ;
            }
            return $"Robot is at [{X},{Y}] facing {Facing}";
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _gridSize && y < _gridSize;
        }
    }
}
