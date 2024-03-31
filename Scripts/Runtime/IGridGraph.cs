namespace Chinchillada.GridGraphs
{
    using System;
    using UnityEngine;

    public interface IGridGraph
    {
        GridGraph.Node this[int x, int y] { get; }
        GridGraph.Node this[Vector2Int cell] { get; }
        int Width  { get; }
        int Height { get; }
    }

    public static class GridGraphExtensions
    {
        public static bool WithinBounds(this IGridGraph graph, Vector2Int coordinate)
        {
            var withinBounds = 0 <= coordinate.x && coordinate.x < graph.Width &&
                               0 <= coordinate.y && coordinate.y < graph.Height;

            return withinBounds;
        }
        
        public static GridGraph.Node GetNeighbor(this IGridGraph graph, GridGraph.Node node, Direction direction)
        {
            return direction switch
            {
                Direction.North => graph.GetNorthNeighbor(node),
                Direction.East  => graph.GetEastNeighbor(node),
                Direction.South => graph.GetSouthNeighbor(node),
                Direction.West  => graph.GetWestNeighbor(node),
                _               => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public static GridGraph.Node GetNorthNeighbor(this IGridGraph graph, GridGraph.Node node)
        {
            return node.Y > 0 ? graph[node.X, node.Y - 1] : null;
        }

        public static GridGraph.Node GetEastNeighbor(this IGridGraph graph, GridGraph.Node node)
        {
            return node.X < graph.Width - 1 ? graph[node.X + 1, node.Y] : null;
        }

        public static GridGraph.Node GetSouthNeighbor(this IGridGraph graph, GridGraph.Node node)
        {
            return node.Y < graph.Height - 1 ? graph[node.X, node.Y + 1] : null;
        }

        public static GridGraph.Node GetWestNeighbor(this IGridGraph graph, GridGraph.Node node)
        {
            return node.X > 0 ? graph[node.X - 1, node.Y] : null;
        }
    }
}