using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Spawner
{
    public class EnemySpawnerInit : IEcsInitSystem
    {
        private readonly EcsPoolInject<EnemySpawnerData> _spawnerData = default;
        private readonly EcsCustomInject<EnemySpawnerSettings> _enemySpawnerSettings = default;

        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var spawner = ref _spawnerData.Value.Add(entity);
            spawner.SpawnerObject = _enemySpawnerSettings.Value.gameObject;
            spawner.EnemyPrefab = _enemySpawnerSettings.Value.EnemyPrefab;
            spawner.MaxSpawnEnemies = _enemySpawnerSettings.Value.MaxSpawn;
            spawner.StartPoolSize = _enemySpawnerSettings.Value.PoolSize;
            spawner.SpawnDelay = _enemySpawnerSettings.Value.SpawnDelay;
            spawner.PlaceSpawnObject = _enemySpawnerSettings.Value.PlaneSpawnObject;
        }
    }
}