using Code.EndGame;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Enemy
{
    public class EnemyMove : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyData>, Exc<EndGameMarker>> _enemyFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                Move(ref enemy);
            }
        }

        private void Move(ref EnemyData enemyData)
        {
            if (enemyData.States == EnemyStates.Idle || !enemyData.EnemyGameObject.gameObject.activeSelf) return;
            enemyData.NavMeshAgent.SetDestination(enemyData.TargetMove.transform.position);
        }
    }
}