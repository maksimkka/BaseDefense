using Code.Logger;
using Code.Pools;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bullet
{
    public sealed class s_ReturnerBulletToPool : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_BulletData, r_ReturnsToThePool>> _returnBulletToPoolFilter = default;
        private readonly EcsFilterInject<Inc<c_WeaponData>> _weaponFilter = default;
        private readonly EcsPoolInject<r_ReturnsToThePool> r_ReturnsRoRhePool = default;

        public void Run(IEcsSystems systems)
        {
            ReturnToPoolGameObject();
        }

        private void ReturnToPoolGameObject()
        {
            foreach (var entity in _returnBulletToPoolFilter.Value)
            {
                r_ReturnsRoRhePool.Value.Del(entity);
                ref var bullet = ref _returnBulletToPoolFilter.Pools.Inc1.Get(entity);
                bullet.BulletRigidBody.velocity = Vector3.zero;
                bullet.CurrentLifeTime = 0;
                Return(bullet.BulletGameObject);
            }
        }

        private void Return(Collider bullet)
        {
            foreach (var entity in _weaponFilter.Value)
            {
                ref var weapon = ref _weaponFilter.Pools.Inc1.Get(entity);

                weapon.BulletsPool.ReturnObject(bullet);
            }
        }
    }
}