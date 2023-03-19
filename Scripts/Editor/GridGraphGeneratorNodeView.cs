namespace Chinchillada.PCGraphs.Editor
{
    using GraphProcessor;
    using GridGraph;
    using UnityEngine.UIElements;

    [NodeCustomEditor(typeof(GridGraphGeneratorNode))]
    public class GridGraphGeneratorNodeView : BaseGeneratorNodeView<GridGraphGeneratorNode, GridGraph>
    {
        private Image previewImage;

        private GridGraphTexturizer texturizer = new GridGraphTexturizer();

        protected override void CreateControls(VisualElement controlContainer)
        {
            base.CreateControls(controlContainer);
            
            this.previewImage = new Image();
            controlContainer.Add(this.previewImage);
        }

        protected override void UpdatePreview(GridGraph nodeResult)
        {
            this.previewImage.image = this.texturizer.Texturize(nodeResult);
        }
    }
}   