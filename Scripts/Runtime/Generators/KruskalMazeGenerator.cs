namespace Chinchillada.GridGraphs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PCGraphs;

    [Serializable]
    public class KruskalMazeGenerator : IGridGraphGenerator
    {
        private Dictionary<GridGraph.Node, int> setByNodes = new Dictionary<GridGraph.Node, int>();

        private Dictionary<int, List<GridGraph.Node>> nodesBySet = new Dictionary<int, List<GridGraph.Node>>();
        
        public IEnumerable<GridGraph> GenerateIterative(int width, int height, IRNG random)
        {
            var grid = new GridGraph(width, height);

            yield return grid;
            
            var connections = GetAllConnections(grid).ToList();
            this.InitializeNodeSets(grid);
            
            while (connections.Any())
            {
                var connection = random.ChooseAndExtract(connections);

                var node     = connection.Node;
                var neighbor = connection.GetOtherNode(grid);

                if (neighbor == null)
                    continue;

                var set         = this.setByNodes[node];
                var neighborSet = this.setByNodes[neighbor];

                if (set == neighborSet)
                    continue;
                
                connection.SetConnect(true);
                this.JoinSets(set, neighborSet);

                yield return grid;
            }
        }

        private static IEnumerable<GridGraph.Connection> GetAllConnections(GridGraph grid)
        {
            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var node = grid[x, y];

                yield return new GridGraph.Connection(node, Direction.East);
                yield return new GridGraph.Connection(node, Direction.South);
            }
        }
        
        private void InitializeNodeSets(GridGraph grid)
        {
            var setID = 0;

            this.setByNodes = new Dictionary<GridGraph.Node, int>();
            this.nodesBySet = new Dictionary<int, List<GridGraph.Node>>();

            for (var x = 0; x < grid.Width; x++)
            for (var y = 0; y < grid.Height; y++)
            {
                var node = grid[x, y];

                this.setByNodes[node]  = setID;
                this.nodesBySet[setID] = new List<GridGraph.Node> {node};

                setID++;
            }
        }

        private void JoinSets(int setA, int setB)
        {
            var lower = setA <= setB ? setA : setB;
            var upper = setA > setB ? setA : setB;

            var lowerNodes = this.nodesBySet[lower];
            var upperNodes = this.nodesBySet[upper];

            lowerNodes.AddRange(upperNodes);

            foreach (var node in upperNodes)
                this.setByNodes[node] = lower;

            this.nodesBySet.Remove(upper);
        }

    }
}