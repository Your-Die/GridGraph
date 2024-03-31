using UnityEngine;

namespace Chinchillada.GridGraphs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;

    public interface ICollectionPicker
    {
        T PickItem<T>(IReadOnlyCollection<T> list);
    }

    [Serializable]
    public class FirstPicker : ICollectionPicker
    {
        public T PickItem<T>(IReadOnlyCollection<T> list) => list.First();
    }

    [Serializable]
    public class LastPicker : ICollectionPicker
    {
        public T PickItem<T>(IReadOnlyCollection<T> list) => list.Last();
    }

    [Serializable]
    public class RandomPicker : ICollectionPicker
    {
        [SerializeReference, Required] private IRNG random = UnityRandom.Shared;
        
        public T PickItem<T>(IReadOnlyCollection<T> list) => this.random.Choose(list);
    }
}