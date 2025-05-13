using UnityEngine;
using System;
using Zenject;
using Object = UnityEngine.Object;

namespace Quiz.Models
{
    public class BaseFactory
    {
        protected DiContainer _container;
        
        public BaseFactory(DiContainer container)
        {
            _container = container;
        }
        
        protected T CreateFromPrefab<T>(Transform parent, Action<T> initialize = null) where T : Object
        {
            var prefab = _container.Resolve<T>();
            var item = Object.Instantiate(prefab, parent);
            initialize?.Invoke(item);
            return item;
        }
    }
}