using Xunit;

namespace ToyRobotSim.Tests
{
    public class GridRendererTests
    {
        // Grid output uses Environment.NewLine, so compare line by line.
        // Line 0 is the "N" label, so the top row of the grid (y = 4) is line 1.
        private static string[] Lines(string grid) => grid.Split(Environment.NewLine);

        [Fact]
        public void Render_BeforePlace_ShowsEmptyGrid()
        {
            var lines = Lines(GridRenderer.Render(new Robot()));

            Assert.Equal(8, lines.Length);
            Assert.Equal("  4 [ ][ ][ ][ ][ ]", lines[1]);
            Assert.Equal("  0 [ ][ ][ ][ ][ ]", lines[5]);
            Assert.Equal("     0  1  2  3  4 ", lines[6]);
        }

        [Fact]
        public void Render_EmptyGrid_ShowsCompassLabels()
        {
            var lines = Lines(GridRenderer.Render(new Robot()));

            // N and S are centred over the middle column, W and E sit either side of the middle row
            Assert.Equal("           N", lines[0]);
            Assert.Equal("W 2 [ ][ ][ ][ ][ ] E", lines[3]);
            Assert.Equal("           S", lines[7]);
        }

        [Fact]
        public void Render_RobotOnTopRow_DrawnOnFirstGridLine()
        {
            var robot = new Robot();
            robot.Place(0, 4, Direction.North);

            var lines = Lines(GridRenderer.Render(robot));

            Assert.Equal("  4 [^][ ][ ][ ][ ]", lines[1]);
        }

        [Fact]
        public void Render_RobotAtOrigin_DrawnBottomLeft()
        {
            var robot = new Robot();
            robot.Place(0, 0, Direction.North);

            var lines = Lines(GridRenderer.Render(robot));

            Assert.Equal("  0 [^][ ][ ][ ][ ]", lines[5]);
        }

        [Theory]
        [InlineData(Direction.North, "W 2 [ ][^][ ][ ][ ] E")]
        [InlineData(Direction.East, "W 2 [ ][>][ ][ ][ ] E")]
        [InlineData(Direction.South, "W 2 [ ][v][ ][ ][ ] E")]
        [InlineData(Direction.West, "W 2 [ ][<][ ][ ][ ] E")]
        public void Render_RobotFacingDirection_ShowsMatchingArrow(Direction facing, string expectedRow)
        {
            var robot = new Robot();
            robot.Place(1, 2, facing);

            var lines = Lines(GridRenderer.Render(robot));

            Assert.Equal(expectedRow, lines[3]);
        }

        [Fact]
        public void Render_CustomGridSize_DrawsThatSize()
        {
            var lines = Lines(GridRenderer.Render(new Robot(3)));

            Assert.Equal(6, lines.Length);
            Assert.Equal("        N", lines[0]);
            Assert.Equal("  2 [ ][ ][ ]", lines[1]);
            Assert.Equal("W 1 [ ][ ][ ] E", lines[2]);
        }
    }
}
