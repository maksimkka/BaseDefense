using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Pools
{
    public class ObjectPool <T> where T : Component
    {
        private readonly Stack<T> objectPool;
        private readonly T prefab;
        private readonly Transform parentTransform;
        private readonly EcsWorld world;

        public ObjectPool(T prefab, Transform parentTransform, EcsWorld world, int initialSize = 10, Action<T> initWithInEcs = null)
        {
            this.prefab = prefab;
            this.parentTransform = parentTransform;
            this.world = world;

            objectPool = new Stack<T>(initialSize);
            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(prefab, parentTransform);
                obj.gameObject.SetActive(false);
                objectPool.Push(obj);
            }
        }

        public T GetObject()
        {
            T obj;
            if (objectPool.Count > 0)
            {
                obj = objectPool.Pop();
            }
            else
            {
                obj = Object.Instantiate(prefab, parentTransform);
            }

            obj.gameObject.SetActive(true);
            return obj;
        }

        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false);
            objectPool.Push(obj);
        }
    }
}