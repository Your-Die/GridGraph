namespace Chinchillada.PCGraph
{
    using System;
    using System.Collections.Generic;
    using GraphProcessor;
    using GridGraph;

    [Serializable]
    public abstract class GridGraphModifierNode : GridGraphGeneratorNode
    {
        [Input] public GridGraph input;

        protected GridGraph Grid { get; private set; }

        protected override IEnumerable<GridGraph> GenerateAsync()
        {
            this.Grid = new GridGraph(this.input);

            return this.Modify(this.Grid);
        }

        protected abstract IEnumerable<GridGraph> Modify(GridGraph grid);
    }
}