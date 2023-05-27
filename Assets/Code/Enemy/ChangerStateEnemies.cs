using Code.EndGame;
using Code.Ground;
using Code.Hero;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public class ChangerStateEnemies : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<CurrentGroundData, ChangeGroundRequest>, Exc<EndGameMarker>> _currentGroundFilter = default;
        private readonly EcsFilterInject<Inc<EnemyData>, Exc<EndGameMarker>> _enemyFilter = default;
        private readonly EcsPoolInject<HeroDetectedMarker> _heroDetectedMarker = default;

        public void Run(IEcsSystems systems)
        {
            ChangeStateEnemies();
            CheckDistance();
        }
        
        private void ChangeStateEnemies()
        {
            foreach (var currentGroundEntity in _currentGroundFilter.Value)
            {
                var currentGroundData = _currentGroundFilter.Pools.Inc1.Get(currentGroundEntity);
                ToggleOnBaseFlag(currentGroundData.IsBaseGround);

                _currentGroundFilter.Pools.Inc2.Del(currentGroundEntity);
            }
        }
        
        private void ToggleOnBaseFlag(bool isHeroOnBase)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                enemy.IsHeroOnBase = isHeroOnBase;
            }
        }

        private void CheckDistance()
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemyData = ref _enemyFilter.Pools.Inc1.Get(entity);
                if(!enemyData.EnemyGameObject.gameObject.activeSelf) continue;
                var distance = Vector3.Distance(enemyData.EnemyGameObject.transform.position, enemyData.TargetMove.transform.position);
                enemyData.CurrentDistance = distance;

                if (enemyData.IsHeroOnBase || distance > enemyData.DetectionDistance)
                {
                    SelectIdleState(ref enemyData, entity);
                }
                
                else if (distance <= enemyData.DetectionDistance)
                {
                    SelectRunState(ref enemyData, entity);
                }
            }
        }

        private void SelectIdleState(ref EnemyData enemyData, int entity)
        {
            if (_heroDetectedMarker.Value.Has(entity))
            {
                enemyData.States = EnemyStates.Idle;
                _heroDetectedMarker.Value.Del(entity);
                enemyData.AnimationSwitcher.PlayAnimation(enemyData.IdleAnimationHash);
                ChangeNavMeshState(ref enemyData, true);
                enemyData.IsReadyAttack = false;
                enemyData.CurrentReloadTime = 0;
            }
        }

        private void SelectRunState(ref EnemyData enemyData, int entity)
        {
            if (!_heroDetectedMarker.Value.Has(entity))
            {
                _heroDetectedMarker.Value.Add(entity);
                enemyData.States = EnemyStates.Run;
                enemyData.AnimationSwitcher.PlayAnimation(enemyData.RunAnimationHash);
                ChangeNavMeshState(ref enemyData, false);
            }
        }

        private void ChangeNavMeshState(ref EnemyData enemyData, bool isStopped)
        {
            if (enemyData.EnemyGameObject.gameObject.activeSelf)
            {
                enemyData.NavMeshAgent.isStopped = isStopped;
            }
        }
    }
}