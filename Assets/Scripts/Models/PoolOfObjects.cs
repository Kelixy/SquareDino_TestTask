using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class PoolOfObjects<T> : ComponentSingleton<PoolOfObjects<T>>
    {
        private readonly Queue<T> _poolOfObjects;
        private readonly Transform _parentTransform;
        private readonly GameObject _prefab;

        public PoolOfObjects(GameObject prefab, Transform parentTransform)
        {
            _poolOfObjects = new Queue<T>();
            _prefab = prefab;
            _parentTransform = parentTransform;
        }

        public void Put(T poolObj)
        {
            _poolOfObjects.Enqueue(poolObj);
        }

        public T Get()
        {
            if (_poolOfObjects.Count <= 0) Create();
            var poolObj = _poolOfObjects.Dequeue();
            return poolObj;
        }

        private void Create()
        {
            var poolObj = Instantiate(_prefab, _parentTransform);
            var component = poolObj.GetComponent<T>();
            Put(component);
        }
    }
}
