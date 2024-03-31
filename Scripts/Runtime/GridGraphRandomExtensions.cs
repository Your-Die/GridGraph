namespace Chinchillada.GridGraphs
{
    public static class GridGraphRandomExtensions
    {
        public static GridGraph.Node ChooseNode(this IRNG random,  GridGraph grid)
        {
            var x = random.Range(0, grid.Width);
            var y = random.Range(0, grid.Height);

            return grid[x, y];
        }
    }
}