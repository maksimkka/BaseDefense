using Code.Bullet;
using Code.Pools;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Weapon
{
    public sealed class i_Bullet : IEcsInitSystem
    {
        private readonly EcsPoolInject<c_BulletData> c_BulletData = default;
        private readonly EcsFilterInject<Inc<c_WeaponData>> _weaponFilter = default;
        private IEcsSystems _system;

        public void Init(IEcsSystems systems)
        {
            _system = systems;
            foreach (var entity in _weaponFilter.Value)
            {
                ref var weapon = ref _weaponFilter.Pools.Inc1.Get(entity);
                var bulletPrefab = weapon.BulletPrefab.GetComponent<Collider>();
                var startPoolSize = weapon.StartPoolSize;
                var bulletsPool = new ObjectPool<Collider>(bulletPrefab, null, startPoolSize, 
                    initWithInEcs: InitBullets);
                weapon.BulletsPool = bulletsPool;
            }
        }

        private void InitBullets(Collider bulletCollider)
        {
            var entity = _system.GetWorld().NewEntity();
            ref var bulletData = ref c_BulletData.Value.Add(entity);
            var bulletSettings = bulletCollider.GetComponent<BulletSettings>();
            bulletData.BulletRigidBody = bulletCollider.GetComponent<Rigidbody>();
            bulletData.BulletGameObject = bulletCollider;
            bulletData.DefaultLifeTime = bulletSettings.DefaultLifeTime;
        }
    }
}