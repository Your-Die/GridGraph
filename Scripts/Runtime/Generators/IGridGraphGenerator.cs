namespace Chinchillada.GridGraphs
{
    using System.Collections.Generic;
    using PCGraphs;

    public interface IGridGraphGenerator
    {
        IEnumerable<GridGraph> GenerateIterative(int width, int height, IRNG random);
    }
}