using Code.EndGame;
using Code.Hero;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyAttack : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyData, HeroDetectedMarker>, Exc<EndGameMarker>> _enemyFilter = default;
        private readonly EcsFilterInject<Inc<HeroData>> _heroFilter = default;
        private readonly EcsPoolInject<SubmitDamageRequest> _submitDamageRequest = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemyData = ref _enemyFilter.Pools.Inc1.Get(entity);
                if (!enemyData.EnemyGameObject.gameObject.activeSelf) continue;
                Reload(ref enemyData);
                Attack(ref enemyData);
            }
        }

        private void Reload(ref EnemyData enemyDataData)
        {
            if (!enemyDataData.IsReadyAttack)
            {
                enemyDataData.CurrentReloadTime += Time.deltaTime;
                if (enemyDataData.CurrentReloadTime >= enemyDataData.DefaultReloadTime)
                {
                    enemyDataData.IsReadyAttack = true;
                }
            }
        }

        private void Attack(ref EnemyData enemyDataData)
        {
            if (enemyDataData.CurrentDistance <= enemyDataData.AttackDistance && enemyDataData.IsReadyAttack)
            {
                enemyDataData.AnimationSwitcher.GetTimeAnimation(enemyDataData.ThrowAnimationHash,
                    enemyDataData.RunAnimationHash);
                enemyDataData.IsReadyAttack = false;
                enemyDataData.CurrentReloadTime = 0;
                SendRequest(enemyDataData.Damage);
            }
        }

        private void SendRequest(int damageDone)
        {
            foreach (var entity in _heroFilter.Value)
            {
                if (!_submitDamageRequest.Value.Has(entity))
                {
                    _submitDamageRequest.Value.Add(entity);
                }

                ref var submitDamageRequest = ref _submitDamageRequest.Value.Get(entity);
                submitDamageRequest.DamageDone = damageDone;
                submitDamageRequest.QueueForDamage++;
            }
        }
    }
}