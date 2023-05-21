using Code.Pools;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Spawner
{
    public class s_EnemiesSpawner : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_EnemySpawnerData>> _filterSpawnEnemy = default;
        private float timerElapsed;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterSpawnEnemy.Value)
            {
                ref var enemySpawnData = ref _filterSpawnEnemy.Pools.Inc1.Get(entity);
                Timer(ref  enemySpawnData);
            }
        }

        private void SpawnObjects(ObjectPool<Collider> enemiesPool, MeshRenderer plane, ref int CountSpawnEnemies)
        {
            var spawnPosition = GetRandomSpawnPosition(plane);
            enemiesPool.GetObject(spawnPosition, Quaternion.identity);
            CountSpawnEnemies++;
        }

        private Vector3 GetRandomSpawnPosition(MeshRenderer plane)
        {
            var planeSize = plane.bounds;
            float randomX = Random.Range(planeSize.min.x, planeSize.max.x);
            float randomZ = Random.Range(planeSize.min.z, planeSize.max.z);

            return new Vector3(randomX, 0f, randomZ);
        }
        private void Timer(ref c_EnemySpawnerData enemySpawnData)
        {
            if(enemySpawnData.CountSpawnEnemies >= enemySpawnData.MaxSpawnEnemies) return;
            timerElapsed += Time.deltaTime;

            if (!(timerElapsed >= enemySpawnData.SpawnDelay)) return;
            timerElapsed = 0;
            SpawnObjects(enemySpawnData.EnemyPool, enemySpawnData.PlaceSpawnObject, ref enemySpawnData.CountSpawnEnemies);
        }
    }
}