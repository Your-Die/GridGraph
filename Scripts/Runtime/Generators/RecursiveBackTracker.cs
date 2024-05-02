using System;

namespace Chinchillada.GridGraphs
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class RecursiveBackTracker : IGridGraphGenerator
    {
        private GridGraph grid;
        private bool[,] visited;

        private readonly IQueue<GridGraph.Node> frontier;
        
        public RecursiveBackTracker(IQueue<GridGraph.Node> frontier)
        {
            this.frontier = frontier;
        }

        public IEnumerable<GridGraph> GenerateIterative(int width, int height, IRNG random)
        {
            this.grid    = new GridGraph(width, height);
            this.visited = new bool[width, height];
            
            this.frontier.Clear();

            yield return this.grid;
            
            var startNode = random.ChooseNode(this.grid);
            this.VisitNode(startNode);

            while (this.frontier.Any())
            {
                var node = this.frontier.Peek();

                var unvisitedDirections = this.GetUnvisitedDirections(node).ToArray();
                if (unvisitedDirections.Length <= 1)
                {
                    this.frontier.Dequeue();

                    if (unvisitedDirections.IsEmpty())
                        continue;
                }

                var direction = random.Choose(unvisitedDirections);
                node.SetConnect(direction);

                var neighbor = this.grid.GetNeighbor(node, direction);
                this.VisitNode(neighbor);

                yield return this.grid;
            }
        }

        private IEnumerable<Direction> GetUnvisitedDirections(GridGraph.Node node)
        {
            var directions = EnumHelper.GetValues<Direction>();
            return directions.Where(IsUnvisited);

            bool IsUnvisited(Direction direction)
            {
                var neighbor = node.Coordinate + direction.ToVector2();
                return this.grid.WithinBounds(neighbor) && !this.IsVisitedAt(neighbor);
            }
        }

        private bool IsVisitedAt(Vector2Int coordinate)
        {
            var isVisitedAt = this.visited[coordinate.x, coordinate.y];
            return isVisitedAt;
        }

        private void VisitNode(GridGraph.Node node)
        {
            this.visited[node.X, node.Y] = true;
            this.frontier.Enqueue(node);
        }
    }
}