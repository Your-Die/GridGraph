namespace Chinchillada.GridGraph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PCGraph;
    using Sirenix.Serialization;
    using UnityEngine;

    [Serializable]
    public class RecursiveBacktracker : IGridGraphGenerator
    {
        [OdinSerialize] private ICollectionPicker nodePicker;

        private bool[,] visited;

        private LinkedList<GridGraph.Node> stack;

        public GridGraph Grid { get; private set; }

        public IEnumerable<GridGraph> GenerateIterative(int width, int height, IRNG random)
        {
            this.Grid    = new GridGraph(width, height);
            this.visited = new bool[width, height];
            this.stack   = new LinkedList<GridGraph.Node>();

            var startNode = random.ChooseNode(this.Grid);
            this.VisitNode(startNode);

            while (this.stack.Any())
            {
                var node = this.ChooseNode();

                var unvisitedDirections = this.GetUnvisitedDirections(node).ToArray();
                if (unvisitedDirections.Length <= 1)
                {
                    this.stack.Remove(node);

                    if (unvisitedDirections.IsEmpty())
                        continue;
                }

                var direction = random.Choose(unvisitedDirections);
                node.SetConnect(direction);

                var neighbor = this.Grid.GetNeighbor(node, direction);
                this.VisitNode(neighbor);

                yield return this.Grid;
            }
        }

        private IEnumerable<Direction> GetUnvisitedDirections(GridGraph.Node node)
        {
            var directions = EnumHelper.GetValues<Direction>();
            return directions.Where(IsUnvisited);

            bool IsUnvisited(Direction direction)
            {
                var neighbor = node.Coordinate + direction.ToVector2();
                return this.Grid.WithinBounds(neighbor) && !this.IsVisitedAt(neighbor);
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
            this.stack.AddLast(node);
        }

        private GridGraph.Node ChooseNode() => this.nodePicker.PickItem(this.stack);
    }
}