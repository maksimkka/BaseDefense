using Code.Pools;
using UnityEngine;

namespace Code.Spawner
{
    public struct EnemySpawnerData
    {
        public ObjectPool<Collider> EnemyPool;
        public GameObject SpawnerObject;
        public GameObject EnemyPrefab;
        public MeshRenderer PlaceSpawnObject;
        public float SpawnDelay;
        public int StartPoolSize;
        public int CountSpawnEnemies;
        public int MaxSpawnEnemies;
    }
}