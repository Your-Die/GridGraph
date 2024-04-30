namespace Chinchillada.PCGraphs
{
    using GridGraphs;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class MazeSpawner : MonoBehaviour, IGrid
    {
        [SerializeField, Required, FindComponent]
        private MazeController mazeController;

        [FindComponent] private IGenerator<GridGraph> mazeGenerator;

        [SerializeReference] private IRNG random;

        private GridGraph maze;

        public int Width => this.mazeController.Width;

        public int Height => this.mazeController.Height;

        [Button]
        public void SpawnMaze()
        {
            this.mazeController.Maze = this.mazeGenerator.Generate(random);
        }

        [Button]
        public void Clear()
        {
            this.mazeController.Maze = null;
        }

        public Vector3 GetCellCenter(int x, int z)
        {
            return this.mazeController.GetCellCenter(x, z);
        }
    }
}