using Code.Ground;
using Code.Hero;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public sealed class s_EnemyMove : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_Enemy>> _enemyFilter;
        private readonly EcsFilterInject<Inc<c_CurrentGroundData, r_ChangeGround>> _currentGroundFilter;
        public void Run(IEcsSystems systems)
        {
            ChangeStateEnemies();
            
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                Move(ref enemy);
            }
        }

        private void Move(ref c_Enemy enemy)
        {
            if(enemy.States == EnemyStates.Idle) return;
            if(!IsCheckDistance(ref enemy)) return;
            enemy.NavMeshAgent.SetDestination(enemy.TargetMove.transform.position);
        }

        private bool IsCheckDistance(ref c_Enemy enemy)
        {
            var distance = Vector3.Distance(enemy.EnemyGameObject.transform.position, enemy.TargetMove.transform.position);
            return distance <= enemy.Distance;
        }

        private void ChangeStateEnemies()
        {
            foreach (var currentGroundEntity in _currentGroundFilter.Value)
            {
                var currentGroundData = _currentGroundFilter.Pools.Inc1.Get(currentGroundEntity);
                if (!currentGroundData.IsBaseGround)
                {
                    SwitchState(EnemyStates.Run, false);
                }

                else
                {
                    SwitchState(EnemyStates.Idle, true);
                }
                
                _currentGroundFilter.Pools.Inc2.Del(currentGroundEntity);
            }
        }

        private void SwitchState(EnemyStates state, bool isStopped)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                enemy.NavMeshAgent.isStopped = isStopped;
                enemy.States = state;
            }
        }
    }
}