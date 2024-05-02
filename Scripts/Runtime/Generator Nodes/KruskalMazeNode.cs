using System;
using System.Collections.Generic;
using GraphProcessor;

namespace Chinchillada.GridGraphs
{
    [Serializable, NodeMenuItem("Grid Graph/Kruskal")]
    public class KruskalMazeNode : GridGraphGeneratorNode, IUsesRNG
    {
        [Input, ShowAsDrawer] public int width;
        [Input, ShowAsDrawer] public int height;

        public IRNG RNG { get; set; }

        public override int ExpectedIterations => this.width * this.height;

        protected override IEnumerable<GridGraph> GenerateAsync()
        {
            var generator = new KruskalMazeGenerator();
            return generator.GenerateIterative(this.width, this.height, this.RNG);
        }
    }
}