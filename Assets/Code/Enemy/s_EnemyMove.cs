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
        public void Run(IEcsSystems systems)
        {

            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                if(!enemy.EnemyGameObject.gameObject.activeSelf) continue;
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
    }
}