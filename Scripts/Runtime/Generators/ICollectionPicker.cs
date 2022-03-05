namespace Chinchillada.GridGraph
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
        [OdinSerialize, Required] private IRNG random = UnityRandom.Shared;
        
        public T PickItem<T>(IReadOnlyCollection<T> list) => this.random.Choose(list);
    }
    
    [Serializable]
    public class CollectionPickerComposite : ICollectionPicker
    {
        [OdinSerialize, Required] private IDistribution<ICollectionPicker> pickers;
        
        [OdinSerialize, Required] private IRNG random = UnityRandom.Shared;
        
        public T PickItem<T>(IReadOnlyCollection<T> list)
        {
            var picker = this.pickers.Sample(this.random);
            return picker.PickItem(list);
        }
    }
}