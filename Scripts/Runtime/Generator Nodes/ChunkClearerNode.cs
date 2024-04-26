using System;
using System.Collections.Generic;
using GraphProcessor;

namespace Chinchillada.PCGraphs.Grid
{
    [Serializable, NodeMenuItem("Grid Graph/Chunk Clearer")]
    public class ChunkClearerNode : IntGridModifierNode
    {
        [Input, ShowAsDrawer] public int empty        = 0;
        [Input, ShowAsDrawer] public int minChunkSize = 1;

        public override int ExpectedIterations => this.inputGrid.Width * this.inputGrid.Height / this.minChunkSize;

        protected override IEnumerable<Grid2D<int>> Modify(Grid2D<int> grid)
        {
            var map = new ChunkMap(grid, this.empty);

            foreach (Chunk chunk in map.Chunks)
            {
                if (chunk.Size >= this.minChunkSize)
                    continue;

                map.ClearChunk(chunk);
                yield return grid;
            }
        }
    }
}