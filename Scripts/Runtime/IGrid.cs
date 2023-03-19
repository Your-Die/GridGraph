namespace Chinchillada.PCGraphs
{
    using UnityEngine;

    public interface IGrid
    {
        int Width  { get; }
        int Height { get; }
        Vector3 GetCellCenter(int x, int z);
    }

    public static class GridExtensions
    {
        public static Vector3 ChooseRandomCell(this IRNG random, IGrid grid)
        {
            var x = random.Range(grid.Width);
            var z = random.Range(grid.Height);

            return grid.GetCellCenter(x, z);
        }
    }
    
}