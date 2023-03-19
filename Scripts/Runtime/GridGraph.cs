namespace Chinchillada.GridGraph
{
    using UnityEngine;

    public partial class GridGraph
    {
        private readonly Node[,] nodes;

        public Node this[Vector2Int cell] => this.nodes[cell.x, cell.y];

        public Node this[int x, int y] => this.nodes[x, y];


        public int Width  => this.nodes.GetLength(0);
        public int Height => this.nodes.GetLength(1);

        public GridGraph(GridGraph toCopy)
        {
            this.nodes = new Node[toCopy.Width, toCopy.Height];

            for (var x = 0; x < this.Width; x++)
            for (var y = 0; y < this.Height; y++)
            {
                var nodeToCopy = toCopy[x, y];

                var node = new Node(this, x, y)
                {
                    IsConnectedEast  = nodeToCopy.IsConnectedEast,
                    IsConnectedSouth = nodeToCopy.IsConnectedSouth
                };

                this.nodes[x, y] = node;
            }
        }

        public GridGraph(int width, int height, bool defaultConnected = false)
        {
            this.nodes = new Node[width, height];

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                var node = new Node(this, x, y)
                {
                    IsConnectedEast  = defaultConnected && x < width  - 1,
                    IsConnectedSouth = defaultConnected && y < height - 1
                };

                this.nodes[x, y] = node;
            }
        }

        public struct Connection
        {
            private readonly Direction direction;

            public Node Node { get; }

            public Connection(Node node, Direction direction)
            {
                this.Node      = node;
                this.direction = direction;
            }

            public Node GetOtherNode(GridGraph graph) => graph.GetNeighbor(this.Node, this.direction);

            public void SetConnect(bool connect) => this.Node.SetConnect(this.direction, connect);
        }
    }
}