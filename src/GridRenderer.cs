using System.Text;

namespace ToyRobotSim
{
    public static class GridRenderer
    {
        // Every line starts with this margin, so the "W" label has room on the middle row
        private const string Margin = "  ";

        // Draws the table with the robot as an arrow showing which way it faces.
        // NORTH is at the top, so rows are printed from the highest Y down to 0.
        public static string Render(Robot robot)
        {
            var sb = new StringBuilder();
            string compassLabelPadding = CompassLabelPadding(robot.GridSize);
            int middleRow = robot.GridSize / 2;

            sb.AppendLine($"{compassLabelPadding}N");

            for (int y = robot.GridSize - 1; y >= 0; y--)
            {
                bool isMiddleRow = y == middleRow;

                sb.Append(isMiddleRow ? "W " : Margin);
                sb.Append($"{y} ");
                for (int x = 0; x < robot.GridSize; x++)
                {
                    bool isRobotHere = robot.IsPlaced && robot.X == x && robot.Y == y;
                    sb.Append(isRobotHere ? $"[{Arrow(robot.Facing)}]" : "[ ]");
                }
                if (isMiddleRow)
                {
                    sb.Append(" E");
                }
                sb.AppendLine();
            }

            sb.Append(Margin);
            sb.Append("  ");
            for (int x = 0; x < robot.GridSize; x++)
            {
                sb.Append($" {x} ");
            }
            sb.AppendLine();

            sb.Append($"{compassLabelPadding}S");

            return sb.ToString();
        }

        // Spaces needed to centre the N and S labels over the grid: the margin, 2 for the
        // row label ("4 "), then half the width of the cells (each cell is 3 wide, "[ ]")
        private static string CompassLabelPadding(int gridSize)
        {
            return new string(' ', Margin.Length + 2 + (gridSize * 3) / 2);
        }

        private static char Arrow(Direction facing)
        {
            switch (facing)
            {
                case Direction.North:
                    return '^';
                case Direction.East:
                    return '>';
                case Direction.South:
                    return 'v';
                case Direction.West:
                    return '<';
                default:
                    throw new ArgumentOutOfRangeException(nameof(facing), facing, null);
            }
        }
    }
}
