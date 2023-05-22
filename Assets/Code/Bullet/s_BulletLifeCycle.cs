using Code.Pools;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bullet
{
    public sealed class s_BulletLifeCycle : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_BulletData>, Exc<r_ReturnsToThePool>> _bulletFilter = default;
        private readonly EcsPoolInject<r_ReturnsToThePool> r_ReturnsToThePool = default;
        
        public void Run(IEcsSystems systems)
        {
            CountDown();
        }

        private void CountDown()
        {
            foreach (var bulletEntity in _bulletFilter.Value)
            {
                ref var bullet = ref _bulletFilter.Pools.Inc1.Get(bulletEntity);

                if (!bullet.BulletGameObject.gameObject.activeSelf) continue;
                bullet.CurrentLifeTime += Time.deltaTime;
                if (bullet.CurrentLifeTime >= bullet.DefaultLifeTime)
                {
                    r_ReturnsToThePool.Value.Add(bulletEntity);
                }
            }
        }
    }
}