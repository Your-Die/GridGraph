namespace Chinchillada.PCGraph
{
    using System.Collections.Generic;
    using GridGraph;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;
    using UnityEngine;

    public class MazeSpawner : SerializedMonoBehaviour, IGrid
    {
        [SerializeField] private float cellSize = 5;

        [SerializeField, Required] private GameObject wallPrefab;
        [SerializeField, Required] private GameObject boundaryPrefab;

        [SerializeField, Required] private Transform topLeft;

        [OdinSerialize, Required] private IGenerator<GridGraph> mazeGenerator;

        [SerializeField, HideInInspector] private List<GameObject> walls = new List<GameObject>();

        private GridGraph maze;
        
        private static Quaternion Horizontal => Quaternion.identity;
        private static Quaternion Vertical   => Quaternion.Euler(0, 90, 0);

        public int Width => maze?.Width ?? 0;
        public int Height => maze?.Height ?? 0;

        [Button]
        public void SpawnMaze()
        {
            this.Clear();

            this.maze = this.mazeGenerator.Generate();

            this.SpawnHorizontalBoundary();
            this.SpawnVerticalBoundary();

            for (var x = 0; x < this.maze.Width; x++)
            for (var y = 0; y < this.maze.Height; y++)
            {
                var node = this.maze[x, y];

                if (x < this.maze.Width - 1 && !node.IsConnectedEast)
                    this.SpawnEastWall(x, y);

                if (y < this.maze.Height - 1 && !node.IsConnectedSouth)
                    this.SpawnSouthWall(x, y);
            }
        }

        [Button]
        public void Clear()
        {
            this.maze = null;
            
            foreach (var wall in this.walls)
                DestroyImmediate(wall);

            this.walls.Clear();
        }

        public Vector3 GetCellCenter(int x, int z)
        {
            var offset = new Vector3
            {
                x = (x + 0.5f) * this.cellSize,
                y = 0,
                z = (z + 0.5f) * this.cellSize
            };

            return this.topLeft.position + offset;
        }

        private void SpawnHorizontalBoundary()
        {
            var top    = 0;
            var bottom = this.cellSize * this.maze.Height;

            for (var x = 0; x < this.maze.Width; x++)
            {
                var left = (x + 0.5f) * this.cellSize;

                this.SpawnBoundary(left, top, Horizontal);
                this.SpawnBoundary(left, bottom, Horizontal);
            }
        }

        private void SpawnVerticalBoundary()
        {
            var left  = 0;
            var right = this.cellSize * this.maze.Width;

            for (var y = 0; y < this.maze.Height; y++)
            {
                var offset = (y + 0.5f) * this.cellSize;

                this.SpawnBoundary(left, offset, Vertical);
                this.SpawnBoundary(right, offset, Vertical);
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

        private void SpawnBoundary(float localX, float localZ, Quaternion rotation)
        {
            this.SpawnPrefab(this.boundaryPrefab, localX, localZ, rotation);
        }

        private void SpawnWall(float localX, float localZ, Quaternion rotation)
        {
            this.SpawnPrefab(this.wallPrefab, localX, localZ, rotation);
        }

        private void SpawnPrefab(GameObject prefab, float localX, float localZ, Quaternion rotation)
        {
            var position = this.topLeft.position;

            position.x += localX;
            position.z += localZ;

            var wall = Instantiate(prefab, position, rotation, this.topLeft);
            this.walls.Add(wall);
        }
    }
}