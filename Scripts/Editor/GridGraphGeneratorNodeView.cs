namespace Chinchillada.PCGraph.Editor
{
    using System.Collections.Generic;
    using GraphProcessor;
    using GridGraph;
    using UnityEngine.UIElements;

    [NodeCustomEditor(typeof(GridGraphGeneratorNode))]
    public class GridGraphGeneratorNodeView : GeneratorNodeView<GridGraphGeneratorNode, GridGraph>
    {
        private Image previewImage;

        private GridGraphTexturizer texturizer = new GridGraphTexturizer();
        
        protected override IEnumerable<VisualElement> CreateControls()
        {
            yield return this.previewImage = new Image();
        }

        protected override void UpdatePreview(GridGraph nodeResult)
        {
            this.previewImage.image = this.texturizer.Texturize(nodeResult);
        }
    }
}   