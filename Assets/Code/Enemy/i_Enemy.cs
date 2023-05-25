using System.Collections.Generic;
using System.Threading;
using Code.Hero;
using Code.Pools;
using Code.Spawner;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    public sealed class i_Enemy : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<c_EnemySpawnerData>> _enemySpawnerFilter = default;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> UnityPhysicsCollisionDataComponent = default;
        private readonly EcsPoolInject<c_Enemy> c_Enemy = default;
        private readonly EcsCustomInject<HeroSettings> _heroSettings = default;
        
        private readonly int _idleAnimation = Animator.StringToHash("DynIdle");
        private readonly int _runAnimation = Animator.StringToHash("Running");
        private readonly int _throwAnimation = Animator.StringToHash("Throw");
        
        private readonly CancellationTokenSource _tokenSources;

        public i_Enemy(CancellationTokenSource tokenSource)
        {
            _tokenSources = tokenSource;
        }
        
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
            var animator = enemySettings.GetComponentInChildren<Animator>();
            ref var enemy = ref c_Enemy.Value.Add(entity);
            enemy.NavMeshAgent = enemyCollider.GetComponent<NavMeshAgent>();
            enemy.EnemyGameObject = enemyCollider;
            enemy.TargetMove = _heroSettings.Value.gameObject;
            enemy.DetectionDistance = enemySettings.DetectionDistance;
            enemy.DefaultReloadTime = enemySettings.CooldownAttack;
            enemy.AttackDistance = enemySettings.AttackDistance;
            enemy.Speed = enemySettings.Speed;
            enemy.DefaultHP = enemySettings.HP;
            enemy.CurrentHP = enemySettings.HP;
            enemy.Damage = enemySettings.Damage;
            enemy.States = EnemyStates.Idle;
            enemy.AnimationSwitcher = new AnimationSwitcher(animator, _tokenSources);
            enemy.IdleAnimationHash = _idleAnimation;
            enemy.RunAnimationHash = _runAnimation;
            enemy.ThrowAnimationHash = _throwAnimation;
            //enemySettings.Entity = entity;
            
            ref var unityPhysicsCollisionDataComponent = ref UnityPhysicsCollisionDataComponent.Value.Add(entity);
            unityPhysicsCollisionDataComponent.CollisionsEnter = new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();
            enemy.Detector = enemySettings.GetComponent<UnityPhysicsCollisionDetector>();
            enemy.Detector.Init(entity, _system.GetWorld());
        }
    }
}