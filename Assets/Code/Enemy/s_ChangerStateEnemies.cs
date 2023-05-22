using Code.Ground;
using Code.Hero;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Enemy
{
    public class s_ChangerStateEnemies : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_CurrentGroundData, r_ChangeGround>> _currentGroundFilter;
        private readonly EcsFilterInject<Inc<c_Enemy>> _enemyFilter;

        public void Run(IEcsSystems systems)
        {
            ChangeStateEnemies();
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
                if (enemy.EnemyGameObject.gameObject.activeSelf)
                {
                    enemy.NavMeshAgent.isStopped = isStopped;
                }
                enemy.States = state;
            }
        }
    }
}