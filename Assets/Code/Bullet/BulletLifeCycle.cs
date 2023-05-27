using Code.Pools;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bullet
{
    public class BulletLifeCycle : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BulletData>, Exc<ReturnsToPoolRequest>> _bulletFilter = default;
        private readonly EcsPoolInject<ReturnsToPoolRequest> _returnsToThePool = default;
        
        public void Run(IEcsSystems systems)
        {
            CountDown();
        }

        private void CountDown()
        {
            foreach (var bulletEntity in _bulletFilter.Value)
            {
                ref var bullet = ref _bulletFilter.Pools.Inc1.Get(bulletEntity);

                if (!bullet.BulletCollider.gameObject.activeSelf) continue;
                bullet.CurrentLifeTime += Time.deltaTime;
                
                if (bullet.CurrentLifeTime >= bullet.DefaultLifeTime)
                {
                    _returnsToThePool.Value.Add(bulletEntity);
                }
            }
        }
    }
}