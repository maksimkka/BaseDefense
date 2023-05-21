using Code.Hero;
using Code.Logger;
using Code.Pools;
using Code.Spawner;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    public sealed class i_Enemy : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<c_EnemySpawnerData>> _enemySpawnerFilter;

        private readonly EcsPoolInject<c_Enemy> c_Enemy;
        
        private readonly EcsCustomInject<HeroSettings> _heroSettings;
        private IEcsSystems _system;

        public void Init(IEcsSystems systems)
        {
            foreach (var entity in _enemySpawnerFilter.Value)
            {
                _system = systems;
                ref var enemySpawnerData = ref _enemySpawnerFilter.Pools.Inc1.Get(entity);
                var enemyPrefab = enemySpawnerData.EnemyPrefab.GetComponent<Collider>();
                var parentObject = enemySpawnerData.SpawnerObject;
                var startPoolSize = enemySpawnerData.StartPoolSize;
                var enemiesPool = new ObjectPool<Collider>(enemyPrefab, parentObject.transform, startPoolSize, 
                    initWithInEcs: InitEnemies);
                enemySpawnerData.EnemyPool = enemiesPool;
            }
        }

        private void InitEnemies(Collider enemyCollider)
        {
            var entity = _system.GetWorld().NewEntity();
            var enemySettings = enemyCollider.GetComponent<EnemySettings>();
            ref var enemy = ref c_Enemy.Value.Add(entity);
            enemy.NavMeshAgent = enemyCollider.GetComponent<NavMeshAgent>();
            enemy.EnemyRigidBody = enemyCollider.GetComponent<Rigidbody>();
            enemy.EnemyGameObject = enemyCollider.gameObject;
            enemy.TargetMove = _heroSettings.Value.gameObject;
            enemy.Distance = enemySettings.Distance;
            enemy.CooldownAttack = enemySettings.CooldownAttack;
            enemy.Speed = enemySettings.Speed;
            enemy.HP = enemySettings.HP;
            enemy.States = EnemyStates.Idle;
        }
    }
}