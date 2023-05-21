using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bullet
{
    public class s_BulletLifeCycle : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_BulletData>> _bulletFilter = default;
        private readonly EcsFilterInject<Inc<c_WeaponData>> _weaponFilter = default;
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
                    bullet.CurrentLifeTime = 0;
                    ReturnToPool(bullet.BulletGameObject);
                }
            }
        }

        private void ReturnToPool(Collider bulletCollider)
        {
            foreach (var entity in _weaponFilter.Value)
            {
                ref var weapon = ref _weaponFilter.Pools.Inc1.Get(entity);
                weapon.BulletsPool.ReturnObject(bulletCollider);
            }
        }
    }
}