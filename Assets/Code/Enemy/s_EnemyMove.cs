using Code.Ground;
using Code.Hero;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public sealed class s_EnemyMove : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_Enemy>> _enemyFilter = default;
        private readonly EcsPoolInject<HeroDetectedMarker> _heroDetectedMarker = default;
        public void Run(IEcsSystems systems)
        {

            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                if(!enemy.EnemyGameObject.gameObject.activeSelf) continue;
                Move(ref enemy, entity);
            }
        }

        private void Move(ref c_Enemy enemy, int enemyEntity)
        {
            if(enemy.States == EnemyStates.Idle) return;
            //if(!IsCheckDistance(ref enemy)) return;
            //var distance = Vector3.Distance(enemy.EnemyGameObject.transform.position, enemy.TargetMove.transform.position);

            //if (distance <= enemy.DetectionDistance)
            //{
            //    if (!_heroDetectedMarker.Value.Has(enemyEntity))
                //{
                    //_heroDetectedMarker.Value.Add(enemyEntity);
            //    }
                //enemy.CurrentDistance = distance;
                //enemy.HeroAnimation.PlayAnimation(enemy.RunAnimationHash);
                enemy.NavMeshAgent.SetDestination(enemy.TargetMove.transform.position);
            //}
            //else
            //{
                // if (_heroDetectedMarker.Value.Has(enemyEntity))
                // {
                //     _heroDetectedMarker.Value.Del(enemyEntity);
                // }
                // enemy.IsReadyAttack = false;
                // enemy.CurrentReloadTime = 0;
                ///enemy.HeroAnimation.PlayAnimation(enemy.RunAnimationHash);
            //}
        }

        // private float IsCheckDistance(ref c_Enemy enemy)
        // {
        //     var distance = Vector3.Distance(enemy.EnemyGameObject.transform.position, enemy.TargetMove.transform.position);
        //
        //     return distance;
        //     //return distance <= enemy.DetectionDistance;
        // }
    }
}