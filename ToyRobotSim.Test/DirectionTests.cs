using Xunit;

namespace ToyRobotSim.Tests
{
    public class DirectionTests
    {
        [Theory]
        [InlineData(Direction.NORTH, Direction.WEST)]
        [InlineData(Direction.WEST, Direction.SOUTH)]
        [InlineData(Direction.SOUTH, Direction.EAST)]
        [InlineData(Direction.EAST, Direction.NORTH)]
        public void TurnLeft_GoesAnticlockwise(Direction start, Direction expected)
        {
            Assert.Equal(expected, start.TurnLeft());
        }

        [Theory]
        [InlineData(Direction.NORTH, Direction.EAST)]
        [InlineData(Direction.EAST, Direction.SOUTH)]
        [InlineData(Direction.SOUTH, Direction.WEST)]
        [InlineData(Direction.WEST, Direction.NORTH)]
        public void TurnRight_GoesClockwise(Direction start, Direction expected)
        {
            Assert.Equal(expected, start.TurnRight());
        }
    }
}
