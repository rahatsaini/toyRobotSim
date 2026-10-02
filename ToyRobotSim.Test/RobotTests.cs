using Xunit;
namespace ToyRobotSim.Tests
{
    public class RobotTests
    {
        [Fact]
        public void Place_OnTable_PlacesRobot()
        {
            var robot = new Robot();
            robot.Place(1, 2, Direction.North);
            var result = robot.Report();
            Assert.Equal("Robot is at [1,2] facing NORTH", result);
        }

        [Theory]
        [InlineData(5, 0)]
        [InlineData(0, 5)]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        public void Place_OffTable_ReturnsInvalidPlacement(int x, int y)
        {
            var robot = new Robot();
            var result = robot.Place(x, y, Direction.North);
            Assert.Equal("Invalid placement", result);
        }

        [Fact]
        public void Commands_BeforePlace_ReturnNotPlaced()
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

        [Theory]
        [InlineData(Direction.North, Direction.West)]
        [InlineData(Direction.West, Direction.South)]
        [InlineData(Direction.South, Direction.East)]
        [InlineData(Direction.East, Direction.North)]
        public void Left_WhenPlaced_TurnsAnticlockwise(Direction start, Direction expected)
        {
            var robot = new Robot();
            robot.Place(0, 0, start);
            robot.Left();
            Assert.Equal(expected, robot.Facing);
        }

        [Theory]
        [InlineData(Direction.North, Direction.East)]
        [InlineData(Direction.East, Direction.South)]
        [InlineData(Direction.South, Direction.West)]
        [InlineData(Direction.West, Direction.North)]
        public void Right_WhenPlaced_TurnsClockwise(Direction start, Direction expected)
        {
            var robot = new Robot();
            robot.Place(0, 0, start);
            robot.Right();
            Assert.Equal(expected, robot.Facing);
        }

        [Theory]
        [InlineData(0, 4, Direction.North)]
        [InlineData(0, 0, Direction.South)]
        [InlineData(4, 0, Direction.East)]
        [InlineData(0, 0, Direction.West)]
        public void Move_OffEdge_DoesNotChangePosition(int x, int y, Direction facing)
        {
            var robot = new Robot();
            robot.Place(x, y, facing);
            robot.Move();
            Assert.Equal(x, robot.X);
            Assert.Equal(y, robot.Y);
        }

        [Fact]
        public void Move_OnTable_MovesOneSquareForward()
        {
            var robot = new Robot();
            robot.Place(1, 1, Direction.North);
            robot.Move();
            Assert.Equal(1, robot.X);
            Assert.Equal(2, robot.Y);
            robot.Right();
            robot.Move();
            Assert.Equal(2, robot.X);
            Assert.Equal(2, robot.Y);
        }

        [Fact]
        public void Constructor_GridSizeZeroOrLess_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Robot(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Robot(-1));
        }

        [Fact]
        public void Place_UndefinedDirection_ThrowsAndLeavesRobotUnplaced()
        {
            var robot = new Robot();

            Assert.Throws<ArgumentOutOfRangeException>(() => robot.Place(0, 0, (Direction)99));
            Assert.False(robot.IsPlaced);
        }

        [Fact]
        public void Place_UndefinedDirectionWhenAlreadyPlaced_KeepsCurrentState()
        {
            var robot = new Robot();
            robot.Place(1, 2, Direction.East);

            Assert.Throws<ArgumentOutOfRangeException>(() => robot.Place(3, 3, (Direction)99));
            Assert.Equal("Robot is at [1,2] facing EAST", robot.Report());
        }
    }
}
