namespace Chinchillada.PCGraphs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GraphProcessor;
    using GridGraph;
    using UnityEngine;

    [Serializable, NodeMenuItem("Grid Graph/Remove Walls")]
    public class SetWallsRandomizedNode : GridGraphModifierNode, IUsesRNG
    {
        [SerializeField, Input, ShowAsDrawer] private bool open;

        [SerializeField, Input, ShowAsDrawer, Range(0, 1)]
        public float percentage;

        private List<GridGraph.Connection> walls;

        private int removeCount;

        public IRNG RNG { get; set; }

        protected override IEnumerable<GridGraph> Modify(GridGraph grid)
        {
            this.walls       = this.GetValidConnections(this.Grid).ToList();
            this.removeCount = Mathf.FloorToInt(this.walls.Count * this.percentage);

            yield return grid;

            for (var i = 0; i < this.removeCount; i++)
            {
                var connection = this.RNG.ChooseAndExtract(this.walls);
                connection.SetConnect(this.open);

                yield return grid;
            }
        }

        private IEnumerable<GridGraph.Connection> GetValidConnections(GridGraph grid)
        {
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var node = grid[x, y];

                if (node.IsConnectedEast == !this.open && x < grid.Width - 1)
                    yield return new GridGraph.Connection(node, Direction.East);

                if (node.IsConnectedSouth == !this.open && y < grid.Height - 1)
                    yield return new GridGraph.Connection(node, Direction.South);
            }
        }
    }
}