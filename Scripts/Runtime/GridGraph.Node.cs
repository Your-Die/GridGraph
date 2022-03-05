namespace Chinchillada.GridGraph
{
    using System;
    using UnityEngine;

    public partial class GridGraph
    {
        public class Node
        {
            private readonly GridGraph graph;

            public int X { get; }
            public int Y { get; }

            public bool IsConnectedEast  { get; set; }
            public bool IsConnectedSouth { get; set; }

            public Vector2Int Coordinate => new Vector2Int(this.X, this.Y);

            public Node(GridGraph graph, int x, int y)
            {
                this.graph = graph;

                this.X = x;
                this.Y = y;
            }

            public override string ToString()
            {
                return $"({this.X}, {this.Y}) [E:{this.IsConnectedEast}, S:{this.IsConnectedSouth}]";
            }

            public void SetConnect(Direction direction, bool connect = true)
            {
                switch (direction)
                {
                    case Direction.North:
                        this.SetConnectNorth(connect);
                        break;
                    case Direction.East:
                        this.SetConnectEast(connect);
                        break;
                    case Direction.South:
                        this.SetConnectSouth(connect);
                        break;
                    case Direction.West:
                        this.SetConnectWest(connect);
                        break;

                    default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }

            private void SetConnectNorth(bool connect)
            {
                if (this.Y == 0)
                    throw new OutOfGridBoundsException();

                var neighbor = this.graph.GetNorthNeighbor(this);
                neighbor.SetConnectSouth(connect);
            }

            private void SetConnectWest(bool connect)
            {
                if (this.X == 0)
                    throw new OutOfGridBoundsException();

                var neighbor = this.graph.GetWestNeighbor(this);
                neighbor.SetConnectEast(connect);
            }

            private void SetConnectEast(bool connect)
            {
                if (this.X == this.graph.Width - 1)
                    throw new OutOfGridBoundsException();

                this.IsConnectedEast = connect;
            }

            private void SetConnectSouth(bool connect)
            {
                if (this.Y == this.graph.Height - 1)
                    throw new OutOfGridBoundsException();

                this.IsConnectedSouth = connect;
            }
        }
    }
}