namespace Chinchillada.GridGraphs
{
    using System;
    using UnityEngine;

    [Serializable]
    public class GridGraphTexturizer
    {
        [SerializeField] private int nodeSize = 9;

        [SerializeField] private int connectorSize = 7;

        [SerializeField] private int spacing = 3;

        [SerializeField] private Color fillColor = Color.black;

        public Texture2D Texturize(GridGraph graph)
        {
            var totalWidth  = this.CalculateSizeTo(graph.Width);
            var totalHeight = this.CalculateSizeTo(graph.Height);

            var texture = new Texture2D(totalWidth, totalHeight);

            for (var x = 0; x < graph.Width; x++)
            for (var y = 0; y < graph.Height; y++)
            {
                var node = graph[x, y];
                this.DrawNode(texture, x, y, node);
            }

            texture.Apply();
            return texture;
        }

        private void DrawNode(Texture2D texture, int x, int y, GridGraph.Node node)
        {
            var offsetX = this.CalculateSizeTo(x);
            var offsetY = this.CalculateSizeTo(y);

            for (var nodeX = 0; nodeX < this.nodeSize; nodeX++)
            for (var nodeY = 0; nodeY < this.nodeSize; nodeY++)
            {
                var pixelX = offsetX + nodeX;
                var pixelY = offsetY + nodeY;

                SetPixel(pixelX, pixelY);
            }

            if (node.IsConnectedSouth)
                DrawSouthConnector();

            if (node.IsConnectedEast)
                DrawEastConnector();

            void DrawEastConnector()
            {
                var startX = offsetX + this.nodeSize;
                var startY = offsetY + (this.nodeSize / 2 - this.connectorSize / 2);

                for (var connectorX = 0; connectorX < this.spacing; connectorX++)
                for (var connectorY = 0; connectorY < this.connectorSize; connectorY++)
                {
                    var pixelX = startX + connectorX;
                    var pixelY = startY + connectorY;

                    SetPixel(pixelX, pixelY);
                }
            }

            void DrawSouthConnector()
            {
                var startX = offsetX + (this.nodeSize / 2 - this.connectorSize / 2);
                var startY = offsetY + this.nodeSize;

                for (var connectorX = 0; connectorX < this.connectorSize; connectorX++)
                for (var connectorY = 0; connectorY < this.spacing; connectorY++)
                {
                    var pixelX = startX + connectorX;
                    var pixelY = startY + connectorY;

                    SetPixel(pixelX, pixelY);
                }
            }

            void SetPixel(int pixelX, int pixelY)
            {
                texture.SetPixel(pixelX, pixelY, this.fillColor);
            }
        }

        private int CalculateSizeTo(int cell)
        {
            if (cell == 0)
                return this.spacing;

            return cell       * this.nodeSize +
                   (cell + 1) * this.spacing;
        }
    }
}