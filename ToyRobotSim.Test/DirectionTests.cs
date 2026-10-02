using Xunit;

namespace ToyRobotSim.Tests
{
    public class DirectionTests
    {
        [Theory]
        [InlineData(Direction.North, Direction.West)]
        [InlineData(Direction.West, Direction.South)]
        [InlineData(Direction.South, Direction.East)]
        [InlineData(Direction.East, Direction.North)]
        public void TurnLeft_EachDirection_TurnsAnticlockwise(Direction start, Direction expected)
        {
            Assert.Equal(expected, start.TurnLeft());
        }

        [Theory]
        [InlineData(Direction.North, Direction.East)]
        [InlineData(Direction.East, Direction.South)]
        [InlineData(Direction.South, Direction.West)]
        [InlineData(Direction.West, Direction.North)]
        public void TurnRight_EachDirection_TurnsClockwise(Direction start, Direction expected)
        {
            Assert.Equal(expected, start.TurnRight());
        }
    }
}
