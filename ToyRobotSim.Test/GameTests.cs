using Xunit;

namespace ToyRobotSim.Tests
{
    public class GameTests
    {
        private static string? Run(params string[] commands)
        {
            var game = new Game();
            string? result = null;
            foreach (var command in commands)
            {
                result = game.Play(command);
            }
            return result;
        }

        [Fact]
        public void Report_BriefExample1_ReturnsZeroOneNorth()
        {
            Assert.Equal("Robot is at [0,1] facing NORTH", Run("PLACE 0,0,NORTH", "MOVE", "REPORT"));
        }

        [Fact]
        public void Report_BriefExample2_ReturnsZeroZeroWest()
        {
            Assert.Equal("Robot is at [0,0] facing WEST", Run("PLACE 0,0,NORTH", "LEFT", "REPORT"));
        }

        [Fact]
        public void Report_BriefExample3_ReturnsThreeThreeNorth()
        {
            Assert.Equal("Robot is at [3,3] facing NORTH",
                Run("PLACE 1,2,EAST", "MOVE", "MOVE", "LEFT", "MOVE", "REPORT"));
        }


        [Fact]
        public void Report_BeforePlace_ReturnsNotPlaced()
        {
            Assert.Equal("Robot is not placed", Run("REPORT"));
        }

        [Fact]
        public void Commands_BeforePlace_ReturnNotPlaced()
        {
            Assert.Equal("Robot is not placed", Run("MOVE", "LEFT", "RIGHT", "REPORT"));
        }

        [Fact]
        public void Place_AfterIgnoredCommands_PlacesRobot()
        {
            Assert.Equal("Robot is at [1,1] facing EAST", Run("MOVE", "LEFT", "PLACE 1,1,EAST", "REPORT"));
        }

        // --- PLACE ---

        [Theory]
        [InlineData("PLACE 5,0,NORTH")]
        [InlineData("PLACE 0,5,NORTH")]
        [InlineData("PLACE -1,0,NORTH")]
        [InlineData("PLACE 0,-1,NORTH")]
        public void Place_OffTable_IsIgnored(string place)
        {
            Assert.Equal("Robot is not placed", Run(place, "REPORT"));
        }

        [Fact]
        public void Place_WhenAlreadyPlaced_MovesRobot()
        {
            Assert.Equal("Robot is at [3,3] facing SOUTH", Run("PLACE 0,0,NORTH", "PLACE 3,3,SOUTH", "REPORT"));
        }

        [Fact]
        public void Place_OffTableWhenAlreadyPlaced_KeepsCurrentPosition()
        {
            Assert.Equal("Robot is at [1,1] facing EAST", Run("PLACE 1,1,EAST", "PLACE 9,9,NORTH", "REPORT"));
        }

        [Theory]
        [InlineData("PLACE 1,2,1")]       // number instead of direction
        [InlineData("PLACE 1,2,UP")]
        [InlineData("PLACE 1,2")]
        [InlineData("PLACE 1,2,")]
        [InlineData("PLACE a,b,NORTH")]
        [InlineData("PLACE")]
        public void Place_Malformed_IsIgnored(string place)
        {
            Assert.Equal("Robot is not placed", Run(place, "REPORT"));
        }

        // --- MOVE: must not fall off any edge ---

        [Theory]
        [InlineData("PLACE 0,4,NORTH", "Robot is at [0,4] facing NORTH")]
        [InlineData("PLACE 4,0,EAST", "Robot is at [4,0] facing EAST")]
        [InlineData("PLACE 0,0,SOUTH", "Robot is at [0,0] facing SOUTH")]
        [InlineData("PLACE 0,0,WEST", "Robot is at [0,0] facing WEST")]
        public void Move_OffEdge_IsIgnored(string place, string expected)
        {
            Assert.Equal(expected, Run(place, "MOVE", "REPORT"));
        }

        [Fact]
        public void Move_AfterIgnoredMove_MovesRobot()
        {
            Assert.Equal("Robot is at [1,0] facing EAST", Run("PLACE 0,0,WEST", "MOVE", "LEFT", "LEFT", "MOVE", "REPORT"));
        }

        // --- Turning ---

        [Fact]
        public void Right_FourTimes_FacesOriginalDirection()
        {
            Assert.Equal("Robot is at [2,2] facing NORTH",
                Run("PLACE 2,2,NORTH", "RIGHT", "RIGHT", "RIGHT", "RIGHT", "REPORT"));
        }

        // --- Input handling ---

        [Fact]
        public void Play_MixedCaseCommands_AreAccepted()
        {
            Assert.Equal("Robot is at [2,3] facing SOUTH", Run("place 2,4,south", "move", "report"));
        }

        [Fact]
        public void Place_SpacesAfterCommas_PlacesRobot()
        {
            Assert.Equal("Robot is at [1,2] facing EAST", Run("PLACE 1, 2, EAST", "REPORT"));
        }

        [Theory]
        [InlineData("MOVE 5")]
        [InlineData("LEFT abc")]
        [InlineData("JUMP")]
        public void Play_UnknownCommandOrExtraWords_IsIgnored(string command)
        {
            Assert.Equal("Robot is at [0,0] facing NORTH", Run("PLACE 0,0,NORTH", command, "REPORT"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Play_EmptyInput_ReturnsNull(string command)
        {
            Assert.Null(new Game().Play(command));
        }

        // --- GRID (extra command, not in the brief) ---

        [Fact]
        public void Grid_AfterMove_ShowsRobotInNewPosition()
        {
            var game = new Game();
            game.Play("PLACE 1,2,EAST");
            game.Play("MOVE");

            var lines = game.Play("GRID")!.Split(Environment.NewLine);

            // Line 0 is the "N" label, so row y = 2 is line 3
            Assert.Equal("W 2 [ ][ ][>][ ][ ] E", lines[3]);
        }
    }
}
