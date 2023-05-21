using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Pools
{
    public sealed class ObjectPool <T> where T : Component
    {
        private readonly Queue<T> objectPool;
        private readonly T prefab;
        private readonly Transform parentTransform;
        private readonly Action<T> _initWithInECS;
        private const int RefillCount = 15;

        public ObjectPool(T prefab, Transform parentTransform, int initialSize = 10, Action<T> initWithInEcs = null)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;
            _initWithInECS = initWithInEcs;

            objectPool = new Queue<T>(initialSize);
            FillPool(initialSize);
        }
        
        private void FillPool(int fillCount)
        {
            for (int i = 0; i < fillCount; i++)
            {
                T obj = Object.Instantiate(prefab, parentTransform);
                _initWithInECS?.Invoke(obj);

                ReturnObject(obj);
            }
        }

        public T GetObject(Vector3 position, Quaternion rotation)
        {
            if (objectPool.Count <= 0)
            {
                FillPool(RefillCount);
            }

            var obj = objectPool.Dequeue();
            var newGameObjectTransform = obj.transform;
            newGameObjectTransform.position = position;
            newGameObjectTransform.rotation = rotation;
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }
}