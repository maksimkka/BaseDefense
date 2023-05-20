﻿using Code.Hero;
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
        private readonly EcsPoolInject<c_Enemy> c_Enemy;
        //private readonly EcsCustomInject<EnemySettings[]> _enemySettings;
        private readonly EcsCustomInject<SpawnerSettings> _spawnerSettings;
        private readonly EcsCustomInject<HeroSettings> _heroSettings;
        public void Init(IEcsSystems systems)
        {
            
            var enemiesPool = new ObjectPool<Collider>(_spawnerSettings.Value.EnemyPrefab.GetComponent<Collider>(), _spawnerSettings.Value.gameObject.transform,systems.GetWorld(), 15, initWithInEcs: InitEnemies);
            // foreach (var enemySettings in _enemySettings.Value)
            // {
            //     var entity = systems.GetWorld().NewEntity();
            //     ref var enemy = ref c_Enemy.Value.Add(entity);
            //     enemy.EnemyGameObject = enemySettings.gameObject;
            //     enemy.NavMeshAgent = enemySettings.GetComponent<NavMeshAgent>();
            //     enemy.EnemyRigidBody = enemySettings.GetComponent<Rigidbody>();
            //     enemy.TargetMove = _heroSettings.Value.gameObject;
            //     enemy.Distance = enemySettings.Distance;
            //     enemy.CooldownAttack = enemySettings.CooldownAttack;
            //     enemy.Speed = enemySettings.Speed;
            //     enemy.HP = enemySettings.HP;
            //     enemy.States = EnemyStates.Idle;
            // }
        }

        private void InitEnemies(Collider enemySettings)
        {
            
        }
    }
}