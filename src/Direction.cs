namespace ToyRobotSim
{
    public enum Direction
    {
        North,
        East,
        South,
        West,
    }

    public static class DirectionExtensions
    {
        public static Direction TurnLeft(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.West;
                case Direction.West:
                    return Direction.South;
                case Direction.South:
                    return Direction.East;
                case Direction.East:
                    return Direction.North;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public static Direction TurnRight(this Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.East;
                case Direction.West:
                    return Direction.North;
                case Direction.South:
                    return Direction.West;
                case Direction.East:
                    return Direction.South;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
