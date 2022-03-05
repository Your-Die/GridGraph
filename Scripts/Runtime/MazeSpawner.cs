namespace Chinchillada.PCGraph
{
    using System.Collections.Generic;
    using GridGraph;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;
    using UnityEngine;

    public class MazeSpawner : SerializedMonoBehaviour
    {
        [SerializeField] private float cellSize = 5;

        [SerializeField, Required] private GameObject wallPrefab;


        [SerializeField, Required] private Transform topLeft;

        [OdinSerialize, Required] private IGenerator<GridGraph> mazeGenerator;

        private readonly List<GameObject> walls = new List<GameObject>();

        private static Quaternion Horizontal => Quaternion.identity;
        private static Quaternion Vertical   => Quaternion.Euler(0, 90, 0);

        [Button]
        public void SpawnMaze()
        {
            this.Clear();

            var maze = this.mazeGenerator.Generate();

            this.SpawnHorizontalBoundary(maze);
            this.SpawnVerticalBoundary(maze);

            for (var x = 0; x < maze.Width; x++)
            for (var y = 0; y < maze.Height; y++)
            {
                var node = maze[x, y];

                if (!node.IsConnectedEast)
                    this.SpawnEastWall(x, y);

                if (!node.IsConnectedSouth)
                    this.SpawnSouthWall(x, y);
            }
        }

        [Button]
        public void Clear()
        {
            foreach (var wall in this.walls)
                Destroy(wall);

            this.walls.Clear();
        }

        private void SpawnHorizontalBoundary(GridGraph maze)
        {
            var top    = 0;
            var bottom = this.cellSize * maze.Height;

            for (var x = 0; x < maze.Width; x++)
            {
                var left = (x + 0.5f) * this.cellSize;

                this.SpawnWall(left, top, Horizontal);
                this.SpawnWall(left, bottom, Horizontal);
            }
        }

        private void SpawnVerticalBoundary(GridGraph maze)
        {
            var left  = 0;
            var right = this.cellSize * maze.Width;

            for (var y = 0; y < maze.Height; y++)
            {
                var offset = (y + 0.5f) * this.cellSize;

                this.SpawnWall(left, offset, Vertical);
                this.SpawnWall(right, offset, Vertical);
            }
        }

        private void SpawnSouthWall(int x, int y)
        {
            var xOffset = (x + 0.5f) * this.cellSize;
            var yOffset = (y + 1f)   * this.cellSize;

            this.SpawnWall(xOffset, yOffset, Horizontal);
        }

        private void SpawnEastWall(int x, int y)
        {
            var xOffset = (x + 1)    * this.cellSize;
            var yOffset = (y + 0.5f) * this.cellSize;

            this.SpawnWall(xOffset, yOffset, Vertical);
        }

        private void SpawnWall(float localX, float localZ, Quaternion rotation)
        {
            var position = this.topLeft.position;

            position.x += localX;
            position.z += localZ;

            var wall = Instantiate(this.wallPrefab, position, rotation, this.topLeft);
            this.walls.Add(wall);
        }
    }
}