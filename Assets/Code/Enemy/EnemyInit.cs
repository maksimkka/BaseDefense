using System.Collections.Generic;
using System.Threading;
using Code.Animations;
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
    public class EnemyInit : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<EnemySpawnerData>> _enemySpawnerFilter = default;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> _unityPhysicsCollisionDataComponent = default;
        private readonly EcsPoolInject<EnemyData> _enemyData = default;
        private readonly EcsCustomInject<HeroSettings> _heroSettings = default;

        private readonly int _idleAnimation = Animator.StringToHash("DynIdle");
        private readonly int _runAnimation = Animator.StringToHash("Running");
        private readonly int _throwAnimation = Animator.StringToHash("Throw");

        private readonly CancellationTokenSource _tokenSources;

        public EnemyInit(CancellationTokenSource tokenSource)
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
            ref var enemy = ref _enemyData.Value.Add(entity);
            enemy.NavMeshAgent = enemyCollider.GetComponent<NavMeshAgent>();
            enemy.NavMeshAgent.speed = enemySettings.Speed;
            enemy.EnemyGameObject = enemyCollider;
            enemy.DetectionDistance = enemySettings.DetectionDistance;
            enemy.DefaultReloadTime = enemySettings.CooldownAttack;
            enemy.AttackDistance = enemySettings.AttackDistance;
            enemy.DefaultHP = enemySettings.HP;
            enemy.CurrentHP = enemySettings.HP;
            enemy.Damage = enemySettings.Damage;
            enemy.States = EnemyStates.Idle;
            enemy.TargetMove = _heroSettings.Value.gameObject;
            enemy.AnimationSwitcher = new AnimationSwitcher(animator, _tokenSources);
            enemy.IdleAnimationHash = _idleAnimation;
            enemy.RunAnimationHash = _runAnimation;
            enemy.ThrowAnimationHash = _throwAnimation;

            ref var unityPhysicsCollisionDataComponent = ref _unityPhysicsCollisionDataComponent.Value.Add(entity);
            unityPhysicsCollisionDataComponent.CollisionsEnter =
                new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();
            enemy.Detector = enemySettings.GetComponent<UnityPhysicsCollisionDetector>();
            enemy.Detector.Init(entity, _system.GetWorld());
        }
    }
}