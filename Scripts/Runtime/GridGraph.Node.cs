namespace Chinchillada.GridGraphs
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public partial class GridGraph : IGridGraph
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

            public IEnumerable<Node> GetConnectedNeighbors()
            {
                if (this.IsConnectedEast)
                    yield return this.graph.GetEastNeighbor(this);

                if (this.IsConnectedSouth)
                    yield return this.graph.GetSouthNeighbor(this);

                var north = this.graph.GetNorthNeighbor(this);
                if (north is { IsConnectedSouth: true })
                    yield return north;

                var west = this.graph.GetWestNeighbor(this);
                if (west is { IsConnectedEast: true })
                    yield return west;
            }

            public IEnumerable<(Node, Direction)> GetConnectedNeighborsWithDirection()
            {
                if (this.IsConnectedEast)
                {
                    Node eastNeighbor = this.graph.GetEastNeighbor(this);
                    yield return (eastNeighbor, Direction.East);
                }
                else if (this.IsConnectedSouth)
                {
                    Node southNeighbor = this.graph.GetSouthNeighbor(this);
                    yield return (southNeighbor, Direction.South);
                }

                Node north = this.graph.GetNorthNeighbor(this);
                if (north is { IsConnectedSouth: true })
                    yield return (north, Direction.North);

                Node west = this.graph.GetWestNeighbor(this);
                if (west is { IsConnectedEast: true })
                    yield return (west, Direction.West);
            }

            public Direction? GetConnection(Node other)
            {
                foreach ((Node neighbor, Direction direction) in this.GetConnectedNeighborsWithDirection())
                {
                    if (other == neighbor)
                        return direction;
                }

                return null;
            }

            public void SetConnect(Direction direction, bool connect = true)
            {
                try
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
                catch (OutOfGridBoundsException exception)
                {
                    Debug.LogException(exception);
                    throw;
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