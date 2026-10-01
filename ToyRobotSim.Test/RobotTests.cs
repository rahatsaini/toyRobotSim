using Xunit;
namespace ToyRobotSim.Tests
{
    public class RobotTests
    {
        [Fact]
        public void Place_ShouldPlaceRobotAtSpecifiedCoordinates()
        {
            var robot = new Robot();
            robot.Place(1, 2, Direction.NORTH);
            var result = robot.Report();
            Assert.Equal("Robot is at [1,2] facing NORTH", result);
        }

        [Theory]
        [InlineData(5, 0)]
        [InlineData(0, 5)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public void Place_ShouldReturnErrorForOutOfBoundsCoordinates(int x, int y)
        {
            var robot = new Robot();
            var result = robot.Place(x, y, Direction.NORTH);
            Assert.Equal("Invalid placement", result);
        }

        [Fact]
        public void Commands_BeforePlace_ShouldReturnError()
        {
            var robot = new Robot();
            var moveResult = robot.Move();
            var leftResult = robot.Left();
            var rightResult = robot.Right();
            var reportResult = robot.Report();
            Assert.Equal("Robot is not placed", moveResult);
            Assert.Equal("Robot is not placed", leftResult);
            Assert.Equal("Robot is not placed", rightResult);
            Assert.Equal("Robot is not placed", reportResult);
        }

        [Fact]
        public void Report_ShouldReturnCurrentPosition()
        {
            var robot = new Robot();
            robot.Place(1, 2, Direction.NORTH);
            var result = robot.Report();
            Assert.Equal("Robot is at [1,2] facing NORTH", result);
        }

        [Theory]
        [InlineData(Direction.NORTH, Direction.WEST)]
        [InlineData(Direction.WEST, Direction.SOUTH)]
        [InlineData(Direction.SOUTH, Direction.EAST)]
        [InlineData(Direction.EAST, Direction.NORTH)]
        public void Left_ShouldTurnRobotLeft(Direction initial, Direction expected)
        {
            var robot = new Robot();
            robot.Place(0, 0, initial);
            robot.Left();
            Assert.Equal(expected, robot.Facing);
        }

        [Theory]
        [InlineData(Direction.NORTH, Direction.EAST)]
        [InlineData(Direction.EAST, Direction.SOUTH)]
        [InlineData(Direction.SOUTH, Direction.WEST)]
        [InlineData(Direction.WEST, Direction.NORTH)]
        public void Right_ShouldTurnRobotRight(Direction initial, Direction expected)
        {
            var robot = new Robot();
            robot.Place(0, 0, initial);
            robot.Right();
            Assert.Equal(expected, robot.Facing);
        }

        [Theory]
        [InlineData(0, 4, Direction.NORTH)]
        [InlineData(0, 0, Direction.SOUTH)]
        [InlineData(4, 0, Direction.EAST)]
        [InlineData(0, 0, Direction.WEST)]
        public void Move_OutOfBounds_ShouldNotChangePosition(int x, int y, Direction facing)
        {
            var robot = new Robot();
            robot.Place(x, y, facing);
            robot.Move();
            Assert.Equal(x, robot.X);
            Assert.Equal(y, robot.Y);
        }

        [Fact]
        public void Robot_ShouldMoveCorrectlyWithinBounds()
        {
            var robot = new Robot();
            robot.Place(1, 1, Direction.NORTH);
            robot.Move();
            Assert.Equal(1, robot.X);
            Assert.Equal(2, robot.Y);
            robot.Right();
            robot.Move();
            Assert.Equal(2, robot.X);
            Assert.Equal(2, robot.Y);
        }

        [Fact]
        public void Robot_ShouldNotHaveGridSizeLessThanOrEqualToZero()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Robot(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Robot(-1));
        }

    }
}
