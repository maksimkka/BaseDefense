using Code.EndGame;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Enemy
{
    public sealed class s_EnemyMove : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_Enemy>, Exc<EndGameMarker>> _enemyFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                if (!enemy.EnemyGameObject.gameObject.activeSelf) continue;
                Move(ref enemy);
            }
        }

        private void Move(ref c_Enemy enemy)
        {
            if (enemy.States == EnemyStates.Idle) return;
            enemy.NavMeshAgent.SetDestination(enemy.TargetMove.transform.position);
        }
    }
}