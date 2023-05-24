using Code.EndGame;
using Code.Hero;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Enemy
{
    public class EnemyAttack : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_Enemy, HeroDetectedMarker>, Exc<EndGameMarker>> _enemyFilter = default;
        private readonly EcsFilterInject<Inc<c_HeroData>> _heroFilter = default;
        private readonly EcsPoolInject<SubmitDamageRequest> SubmitDamageRequest;

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
                    enemyData.AnimationSwitcher.GetTimeAnimation(enemyData.ThrowAnimationHash, enemyData.RunAnimationHash);
                    enemyData.IsReadyAttack = false;
                    enemyData.CurrentReloadTime = 0;
                    SendRequest(enemyData.Damage);
                }
            }
        }

        private void SendRequest(int damageDone)
        {
            foreach (var entity in _heroFilter.Value)
            {
                if (!SubmitDamageRequest.Value.Has(entity))
                {
                    SubmitDamageRequest.Value.Add(entity);
                }

                ref var submitDamageRequest = ref SubmitDamageRequest.Value.Get(entity);
                submitDamageRequest.DamageDone = damageDone;
                submitDamageRequest.QueueForDamage++;
            }
        }
    }
}