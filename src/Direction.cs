namespace ToyRobotSim
{

    public static class DirectionExtensions
    {
        public static Direction TurnLeft(this Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return Direction.WEST;
                case Direction.WEST:
                    return Direction.SOUTH;
                case Direction.SOUTH:
                    return Direction.EAST;
                case Direction.EAST:
                    return Direction.NORTH;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
        public static Direction TurnRight(this Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return Direction.EAST;
                case Direction.WEST:
                    return Direction.NORTH;
                case Direction.SOUTH:
                    return Direction.WEST;
                case Direction.EAST:
                    return Direction.SOUTH;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
