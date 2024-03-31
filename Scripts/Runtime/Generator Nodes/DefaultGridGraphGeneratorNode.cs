namespace Chinchillada.GridGraphs
{
    using System;
    using System.Collections.Generic;
    using GraphProcessor;
    using UnityEngine;

    [Serializable, NodeMenuItem("Grid Graph/Default")]
    public class DefaultGridGraphGeneratorNode : GridGraphGeneratorNode
    {
        [SerializeField, Input, ShowAsDrawer] public int width  = 6;
        [SerializeField, Input, ShowAsDrawer] public int height = 6;

        [SerializeField, Input, ShowAsDrawer] public bool defaultConnected;

        protected override IEnumerable<GridGraph> GenerateAsync()
        {
            yield return new GridGraph(this.width, this.height, this.defaultConnected); 
        }
    }
}