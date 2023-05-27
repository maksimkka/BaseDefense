using Code.Bonus;
using Code.Spawner;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyHitHandling : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyData, TakingDamageRequest>> _enemyFilter = default;
        private readonly EcsFilterInject<Inc<EnemySpawnerData>> _spawnerFilter = default;
        private readonly EcsFilterInject<Inc<BonusSpawnerData>> _bonusSpawnerDataFilter = default;
        private readonly EcsPoolInject<TakingDamageRequest> _takingDamageRequest = default;
        private readonly EcsPoolInject<SpawnBonusRequest> _spawnBonusRequest = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                ref var takingDamage = ref _enemyFilter.Pools.Inc2.Get(entity);
                enemy.CurrentHP -= takingDamage.Damage;

                if (enemy.CurrentHP <= 0)
                {
                    enemy.CurrentHP = enemy.DefaultHP;
                    ReturnToPool(enemy.EnemyGameObject);
                }

                _takingDamageRequest.Value.Del(entity);
            }
        }

        private void ReturnToPool(Collider enemyObject)
        {
            foreach (var entity in _spawnerFilter.Value)
            {
                ref var spawner = ref _spawnerFilter.Pools.Inc1.Get(entity);
                spawner.EnemyPool.ReturnObject(enemyObject, spawner.SpawnerObject.transform);
                spawner.CountSpawnEnemies--;
                SpawnBonuses(enemyObject.transform.position);

                if (spawner.CountSpawnEnemies <= 0)
                {
                    spawner.CountSpawnEnemies = 0;
                }
            }
        }

        private void SpawnBonuses(Vector3 spawnPosition)
        {
            foreach (var entity in _bonusSpawnerDataFilter.Value)
            {
                ref var spawnBonusRequest = ref _spawnBonusRequest.Value.Add(entity);
                spawnBonusRequest.SpawnPosition = spawnPosition;
            }
        }
    }
}