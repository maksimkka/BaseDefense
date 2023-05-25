using Code.Bonus;
using Code.Logger;
using Code.Spawner;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public sealed class s_HitHandling : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_Enemy, r_TakingDamage>> _enemyFilter = default;
        private readonly EcsFilterInject<Inc<c_EnemySpawnerData>> _spawnerFilter = default;
        private readonly EcsFilterInject<Inc<BonusSpawnerData>> _bonusSpawnerDataFilter = default;
        private readonly EcsPoolInject<r_TakingDamage> r_TakingDamage = default;
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
                    //enemy.States = EnemyStates.Idle;
                    //enemy.NavMeshAgent.isStopped = true;
                    ReturnToPool(enemy.EnemyGameObject);
                }
                r_TakingDamage.Value.Del(entity);
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