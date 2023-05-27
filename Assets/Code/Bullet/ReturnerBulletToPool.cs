using Code.Pools;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bullet
{
    public class ReturnerBulletToPool : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BulletData, ReturnsToPoolRequest>> _returnBulletToPoolFilter = default;
        private readonly EcsFilterInject<Inc<WeaponData>> _weaponFilter = default;
        private readonly EcsPoolInject<ReturnsToPoolRequest> _returnsRoRhePool = default;

        public void Run(IEcsSystems systems)
        {
            ReturnToPoolGameObject();
        }

        private void ReturnToPoolGameObject()
        {
            foreach (var entity in _returnBulletToPoolFilter.Value)
            {
                _returnsRoRhePool.Value.Del(entity);
                ref var bullet = ref _returnBulletToPoolFilter.Pools.Inc1.Get(entity);
                bullet.BulletRigidBody.velocity = Vector3.zero;
                bullet.CurrentLifeTime = 0;
                Return(bullet.BulletCollider);
            }
        }

        private void Return(Collider bullet)
        {
            foreach (var entity in _weaponFilter.Value)
            {
                ref var weaponData = ref _weaponFilter.Pools.Inc1.Get(entity);

                weaponData.BulletsPool.ReturnObject(bullet, weaponData.WeaponParent);
            }
        }
    }
}