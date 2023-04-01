namespace Chinchillada.GridGraph
{
    using System;
    using System.Collections.Generic;
    using GraphProcessor;
    using PCGraphs;
    using UnityEngine;

    [Serializable, NodeMenuItem("Grid Graph/Maze Generator")]
    public class MazeGeneratorNode : GridGraphGeneratorNode, IUsesRNG
    {
        [SerializeField, Input, ShowAsDrawer] public int width;
        [SerializeField, Input, ShowAsDrawer] public int height;

        [SerializeField, Input, ShowAsDrawer] private GridGraphGeneratorAsset generator;

        public IRNG RNG { get; set; }

        public override int ExpectedIterations => this.width * this.height;

        protected override IEnumerable<GridGraph> GenerateAsync()
        {
            return this.generator.GenerateIterative(this.width, this.height, this.RNG);
        }
    }
}