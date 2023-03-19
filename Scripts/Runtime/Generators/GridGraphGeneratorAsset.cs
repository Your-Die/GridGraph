namespace Chinchillada.GridGraph
{
    using System.Collections.Generic;
    using PCGraphs;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    public class GridGraphGeneratorAsset : SerializedScriptableObject, IGridGraphGenerator
    {
        [OdinSerialize, Required] private IGridGraphGenerator generator;
        
        public IEnumerable<GridGraph> GenerateIterative(int width, int height, IRNG random)
        {
            return this.generator.GenerateIterative(width, height, random);
        }
    }
}