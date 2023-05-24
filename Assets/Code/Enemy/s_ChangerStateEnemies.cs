using Code.Ground;
using Code.Hero;
using Code.Logger;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public sealed class s_ChangerStateEnemies : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_CurrentGroundData, r_ChangeGround>> _currentGroundFilter = default;
        private readonly EcsFilterInject<Inc<c_Enemy>> _enemyFilter = default;
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
                SwitchState(currentGroundData.IsBaseGround);

                _currentGroundFilter.Pools.Inc2.Del(currentGroundEntity);
            }
        }
        
        private void SwitchState(bool isHeroOnBase)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                enemy.IsHeroOnBase = isHeroOnBase;
                // if (enemy.EnemyGameObject.gameObject.activeSelf)
                // {
                //     enemy.NavMeshAgent.isStopped = isStopped;
                //
                //     enemy.HeroAnimation.PlayAnimation(isStopped ? enemy.IdleAnimationHash : enemy.RunAnimationHash);
                // }
                // enemy.States = state;
            }
        }

        // private void SwitchState(bool isStopped)
        // {
        //     foreach (var entity in _enemyFilter.Value)
        //     {
        //         ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
        //         if (enemy.EnemyGameObject.gameObject.activeSelf)
        //         {
        //             enemy.NavMeshAgent.isStopped = isStopped;
        //
        //             enemy.HeroAnimation.PlayAnimation(isStopped ? enemy.IdleAnimationHash : enemy.RunAnimationHash);
        //         }
        //         enemy.States = state;
        //     }
        // }

        private void CheckDistance()
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                var distance = Vector3.Distance(enemy.EnemyGameObject.transform.position, enemy.TargetMove.transform.position);
                enemy.CurrentDistance = distance;

                if (enemy.IsHeroOnBase || distance > enemy.DetectionDistance)
                {
                    if (_heroDetectedMarker.Value.Has(entity))
                    {
                        enemy.States = EnemyStates.Idle;
                        _heroDetectedMarker.Value.Del(entity);
                        enemy.HeroAnimation.PlayAnimation(enemy.IdleAnimationHash);
                        ChangeNavMeshState(ref enemy, true);
                        $"123123123123".Colored(Color.yellow).Log();
                        enemy.IsReadyAttack = false;
                        enemy.CurrentReloadTime = 0;
                    }
                }
                
                else if (distance <= enemy.DetectionDistance)
                {
                    if (!_heroDetectedMarker.Value.Has(entity))
                    {
                        _heroDetectedMarker.Value.Add(entity);
                        enemy.States = EnemyStates.Run;
                        enemy.HeroAnimation.PlayAnimation(enemy.RunAnimationHash);
                        ChangeNavMeshState(ref enemy, false);
                    }
                }
                
                
                //if (distance <= enemy.DetectionDistance && enemy.States != EnemyStates.Idle)
                //{
                    // if (!_heroDetectedMarker.Value.Has(entity))
                    // {
                    //     _heroDetectedMarker.Value.Add(entity);
                    // }
                    //enemy.HeroAnimation.PlayAnimation(enemy.RunAnimationHash);
                //}
                //else
                //{
                    // if (_heroDetectedMarker.Value.Has(entity))
                    // {
                    //     _heroDetectedMarker.Value.Del(entity);
                    // }
                    // enemy.IsReadyAttack = false;
                    // enemy.CurrentReloadTime = 0;
                    //enemy.HeroAnimation.PlayAnimation(enemy.RunAnimationHash);
                //}
            }
        }

        private void ChangeNavMeshState(ref c_Enemy enemy, bool isStopped)
        {
            if (enemy.EnemyGameObject.gameObject.activeSelf)
            {
                enemy.NavMeshAgent.isStopped = isStopped;
            }
            
        }

        private void AddOrDeleteHeroDetectorMarker(int entity)
        {
            if (!_heroDetectedMarker.Value.Has(entity))
            {
                _heroDetectedMarker.Value.Add(entity);
            }
            else
            {
                _heroDetectedMarker.Value.Del(entity);
            }
        }
    }
}