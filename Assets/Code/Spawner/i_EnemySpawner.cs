using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Spawner
{
    public sealed class i_EnemySpawner : IEcsInitSystem
    {
        private readonly EcsPoolInject<c_EnemySpawnerData> c_Spawner = default;
        private readonly EcsCustomInject<EnemySpawnerSettings> _enemySpawnerSettings = default;

        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var spawner = ref c_Spawner.Value.Add(entity);
            spawner.SpawnerObject = _enemySpawnerSettings.Value.gameObject;
            spawner.EnemyPrefab = _enemySpawnerSettings.Value.EnemyPrefab;
            spawner.MaxSpawnEnemies = _enemySpawnerSettings.Value.MaxSpawn;
            spawner.StartPoolSize = _enemySpawnerSettings.Value.PoolSize;
            spawner.SpawnDelay = _enemySpawnerSettings.Value.SpawnDelay;
            spawner.PlaceSpawnObject = _enemySpawnerSettings.Value.PlaneSpawnObject;
        }
    }
}