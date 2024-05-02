using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using GraphProcessor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Chinchillada.GridGraphs
{
    [Serializable, NodeMenuItem("Grid Graph/Recursive Backtracker")]
    public class RecursiveBacktrackerMazeNode : GridGraphGeneratorNode, IUsesRNG
    {
        [SerializeField, Input, ShowAsDrawer] public int width;
        [SerializeField, Input, ShowAsDrawer] public int height;

        public BacktrackStrategy strategy; 
        
        public IRNG RNG { get; set; }

        public override int ExpectedIterations => this.width * this.height;

        protected override IEnumerable<GridGraph> GenerateAsync()
        {
            var backTracker = this.CreateBackTracker();
            return backTracker.GenerateIterative(this.width, this.height, this.RNG);
        }

        private RecursiveBackTracker CreateBackTracker()
        {
            IQueue<GridGraph.Node> frontier =  this.strategy switch
            {
                BacktrackStrategy.Fifo => new FifoQueue<GridGraph.Node>(),
                BacktrackStrategy.Lifo => new LifoQueue<GridGraph.Node>(),
                BacktrackStrategy.Random => new RandomQueue<GridGraph.Node>(this.RNG),
                _ => throw new ArgumentOutOfRangeException()
            };

            return new RecursiveBackTracker(frontier);
        }

        public enum BacktrackStrategy
        {
            Fifo,
            Lifo,
            Random
        }
    }
}