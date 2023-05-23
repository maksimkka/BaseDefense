using Code.Logger;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyAttack : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_Enemy, HeroDetectedMarker>> _enemyFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemyData = ref _enemyFilter.Pools.Inc1.Get(entity);
                Reload(ref enemyData);
                Attack(ref enemyData);
            }
        }

        private void Reload(ref c_Enemy enemyData)
        {
            if (!enemyData.IsReadyAttack)
            {
                enemyData.CurrentReloadTime += Time.deltaTime;
                if (enemyData.CurrentReloadTime >= enemyData.DefaultReloadTime)
                {
                    enemyData.IsReadyAttack = true;
                }
            }
        }

        private void Attack(ref c_Enemy enemyData)
        {
            if (enemyData.CurrentDistance <= enemyData.AttackDistance)
            {
                if (enemyData.IsReadyAttack)
                {
                    "YEBAL".Colored(Color.cyan).Log();
                    enemyData.IsReadyAttack = false;
                    enemyData.CurrentReloadTime = 0;
                }
            }
        }
    }
}