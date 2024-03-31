using UnityEngine;

namespace Chinchillada.GridGraphs
{
    using System.Collections.Generic;
    using PCGraphs;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    public class GridGraphGeneratorAsset : ScriptableObject, IGridGraphGenerator
    {
        [SerializeReference, Required] private IGridGraphGenerator generator;
        
        public IEnumerable<GridGraph> GenerateIterative(int width, int height, IRNG random)
        {
            return this.generator.GenerateIterative(width, height, random);
        }
    }
}