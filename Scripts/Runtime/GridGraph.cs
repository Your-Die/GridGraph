namespace Chinchillada.GridGraph
{
    using System;
    using Sirenix.Serialization;
    using UnityEngine;

    [Serializable]
    public partial class GridGraph
    {
        [OdinSerialize] private Node[,] nodes;

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

        public bool WithinBounds(Vector2Int coordinate)
        {
            var withinBounds = 0 <= coordinate.x && coordinate.x < this.Width &&
                               0 <= coordinate.y && coordinate.y < this.Height;

            return withinBounds;
        }

        public Node GetNeighbor(Node node, Direction direction)
        {
            return direction switch
            {
                Direction.North => this.GetNorthNeighbor(node),
                Direction.East  => this.GetEastNeighbor(node),
                Direction.South => this.GetSouthNeighbor(node),
                Direction.West  => this.GetWestNeighbor(node),
                _               => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public Node GetNorthNeighbor(Node node) => node.Y > 0 ? this[node.X, node.Y - 1] : null;

        public Node GetEastNeighbor(Node node) => node.X < this.Width - 1 ? this[node.X + 1, node.Y] : null;

        public Node GetSouthNeighbor(Node node) => node.Y < this.Height - 1 ? this[node.X, node.Y + 1] : null;

        public Node GetWestNeighbor(Node node) => node.X > 0 ? this[node.X - 1, node.Y] : null;

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