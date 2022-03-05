namespace Chinchillada.GridGraph
{
    using System.Collections.Generic;
    using PCGraph;

    public interface IGridGraphGenerator
    {
        IEnumerable<GridGraph> GenerateIterative(int width, int height, IRNG random);
    }
}