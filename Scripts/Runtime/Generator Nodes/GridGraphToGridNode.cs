using System;
using System.Collections.Generic;
using Chinchillada.Grid;
using Chinchillada.GridGraphs;
using Chinchillada.Grid.PCGraphs;
using GraphProcessor;

namespace Chinchillada
{
    [Serializable, NodeMenuItem("GridGraph/To Grid")]
    public class GridGraphToGridNode : IntGridGeneratorNode
    {
        [Input] public GridGraph gridGraph;

        [Input, ShowAsDrawer] public int openValue = 0;
        [Input, ShowAsDrawer] public int wallValue = 1;
        
        protected override IEnumerable<Grid2D<int>> GenerateAsync()
        {
            int graphWidth = this.gridGraph.Width;
            int graphHeight = this.gridGraph.Height;

            int width = graphWidth + graphWidth - 1;
            int height = graphHeight + graphHeight - 1;

            var grid = new Grid2D<int>(width, height, this.openValue);
            
            for (int x = 0; x < graphWidth; x++)
            for (int y = 0; y < graphHeight; y++)
            {
                GridGraph.Node node = this.gridGraph[x, y];
                (int gridX, int gridY) = GridGraphToGrid(x, y);
                
                if (node.IsConnectedEast)
                {
                    for (int xOffset = 0; xOffset <= 2; xOffset++)
                    {
                        int finalX = gridX + xOffset;
                        grid[finalX, gridY] = this.wallValue;
                    }
                }

                if (node.IsConnectedSouth)
                {
                    for (int yOffset = 0; yOffset <= 2; yOffset++)
                    {
                        int finalY = gridY + yOffset;
                        grid[gridX, finalY] = this.wallValue;
                    }
                }
            }

            yield return grid;
            
            (int x, int y) GridGraphToGrid(int x, int y) => (x * 2, y * 2);
        }
    }
}