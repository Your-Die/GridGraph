using System;
using System.Collections.Generic;
using System.Linq;
using GraphProcessor;
using UnityEngine;

namespace Chinchillada.PCGraphs.Grid
{
    [Serializable, NodeMenuItem("Grid Graph/Chunk Cutter")]
    public class ChunkCutterNode : IntGridModifierNode, IUsesRNG
    {
        [Input, ShowAsDrawer] public int          empty        = 0;
        [Input, ShowAsDrawer] public int          minChunkSize = 4;
        [Input, ShowAsDrawer] public int          maxChunkSize = 4;
        
        [Input, ShowAsDrawer] public Neighborhood neighborhood = Neighborhood.Diagonal;

        public IRNG RNG { get; set; }

        public override int ExpectedIterations => this.inputGrid.Width * this.inputGrid.Height / this.minChunkSize;

        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            if (this.minChunkSize > this.maxChunkSize)
                throw new ArgumentException($"{nameof(this.minChunkSize)} cannot exceed {this.maxChunkSize}");

            var chunkMap = new ChunkMap(grid, this.empty, this.neighborhood);

            yield return grid;

            var tooBigChunks = chunkMap.Chunks.Where(TooBig).ToList();

            while (tooBigChunks.Any())
            {
                Chunk chunk = tooBigChunks.GrabRandom(this.RNG);

                var subChunks = chunkMap.CutRandomSubChunk(chunk, this.minChunkSize, this.RNG);
                foreach (Chunk subChunk in subChunks)
                {
                    if (TooBig(subChunk))
                    {
                        tooBigChunks.Add(subChunk);
                    }
                }

                yield return grid;
            }

            bool TooBig(Chunk chunk) => chunk.Size > this.maxChunkSize;
        }
    }

    public class Chunk
    {
        public int ID { get; }

        public List<Vector2Int> Coordinates { get; }

        public int Size => this.Coordinates.Count;

        public Chunk(int id) : this(id, new List<Vector2Int>())
        {
        }

        public Chunk(int id, List<Vector2Int> coordinates)
        {
            this.ID = id;
            this.Coordinates = coordinates;
        }

        public void ApplyID(Grid2D<int> map)
        {
            foreach (Vector2Int coordinate in this.Coordinates)
                map[coordinate] = this.ID;
        }

        public void AddCoordinate(Vector2Int coordinate) => this.Coordinates.Add(coordinate);

        public bool Contains(Vector2Int coordinate) => this.Coordinates.Contains(coordinate);

        public override string ToString() => $"ID: {this.ID}, Size{this.Size}";
    }

    public class ChunkMap
    {
        private readonly Grid2D<int> grid;

        private readonly int empty;

        private readonly Neighborhood neighborhood;


        private readonly Grid2D<int> map;
        private readonly Dictionary<int, Chunk> chunksByID = new();

        private int nextUnusedID = 1;

        public IEnumerable<Chunk> Chunks => this.chunksByID.Values;

        public ChunkMap(Grid2D<int> grid, int empty, Neighborhood neighborhood = Neighborhood.Diagonal)
        {
            this.grid = grid;
            this.empty = empty;
            this.neighborhood = neighborhood;

            this.map = grid.Select(x => x == empty ? empty : -1);

            this.ReadCurrentChunks();
        }

        public List<Chunk> CutRandomSubChunk(Chunk chunk, int size, IRNG random)
        {
            var output = new List<Chunk>();

            if (chunk.Size < size)
            {
                output.Add(chunk);
                return output;
            }

            this.chunksByID.Remove(chunk.ID);
            Chunk subChunk = this.GenerateRandomSubChunk(chunk, size, random);

            foreach (Vector2Int coordinate in subChunk.Coordinates)
            {
                var connectionsToChunk = this.map.GetNeighborsWithValue(coordinate, chunk.ID, this.neighborhood);

                foreach (Vector2Int connection in connectionsToChunk)
                {
                    this.ClearCoordinate(connection);
                    var remainders = this.map.GetNeighborsWithValue(connection, chunk.ID, this.neighborhood);

                    foreach (Vector2Int remainder in remainders)
                    {
                        Chunk remainderChunk = this.CreateChunkAt(remainder);
                        this.chunksByID.Add(remainderChunk.ID, remainderChunk);

                        output.Add(remainderChunk);
                    }
                }
            }

            return output;
        }

        public void ClearChunk(Chunk chunk)
        {
            foreach (Vector2Int coordinate in chunk.Coordinates)
                this.ClearCoordinate(coordinate);
        }

        private void ClearCoordinate(Vector2Int coordinate)
        {
            this.map[coordinate] = this.empty;
            this.grid[coordinate] = this.empty;
        }

        private Chunk GenerateRandomSubChunk(Chunk chunk, int size, IRNG random)
        {
            Vector2Int subChunkOrigin = chunk.Coordinates.ChooseRandom(random);
            Chunk subChunk = this.CreateEmptyChunk();

            var frontier = new List<Vector2Int> { subChunkOrigin };
            while (frontier.Any() && subChunk.Size < size)
            {
                Vector2Int coordinate = frontier.GrabRandom(random);
                subChunk.AddCoordinate(coordinate);

                var neighbors = this.map.GetNeighborsWithValue(coordinate, chunk.ID);

                foreach (Vector2Int neighbor in neighbors)
                {
                    if (!subChunk.Contains(neighbor) && !frontier.Contains(neighbor))
                        frontier.Add(neighbor);
                }
            }

            subChunk.ApplyID(this.map);
            this.chunksByID.Add(subChunk.ID, chunk);

            return subChunk;
        }

        private void ReadCurrentChunks()
        {
            this.ForgetChunks();

            foreach (Vector2Int coordinate in this.map.Coordinates)
            {
                int value = this.map[coordinate];
                if (value == this.empty || this.chunksByID.ContainsKey(value))
                    continue;

                Chunk chunk = this.CreateChunkAt(coordinate);
                this.chunksByID[chunk.ID] = chunk;
            }
        }

        private Chunk CreateChunkAt(Vector2Int coordinate)
        {
            int canvas = this.map[coordinate];

            int chunkID = this.GetNextID();
            var coordinates = this.map.FloodFill(coordinate, canvas, chunkID, this.neighborhood);

            return new Chunk(chunkID, coordinates);
        }

        private Chunk CreateEmptyChunk()
        {
            int id = this.GetNextID();
            return new Chunk(id);
        }

        private int GetNextID() => this.nextUnusedID++;

        private void ForgetChunks()
        {
            this.chunksByID.Clear();
            this.nextUnusedID = 1;
        }
    }
}