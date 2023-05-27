using System.Collections.Generic;
using Code.Pools;
using Code.UnityPhysics;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bullet
{
    public class BulletInit : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<WeaponData>> _weaponFilter = default;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> _unityPhysicsCollisionDataComponent = default;
        private readonly EcsPoolInject<BulletData> _bulletData = default;

        private IEcsSystems _system;

        public void Init(IEcsSystems systems)
        {
            _system = systems;
            foreach (var entity in _weaponFilter.Value)
            {
                ref var weapon = ref _weaponFilter.Pools.Inc1.Get(entity);
                var bulletPrefab = weapon.BulletPrefab.GetComponent<Collider>();
                var startPoolSize = weapon.StartPoolSize;
                var bulletsPool = new ObjectPool<Collider>(bulletPrefab, weapon.WeaponParent, startPoolSize, 
                    initWithInEcs: InitBullets);
                weapon.BulletsPool = bulletsPool;
            }
        }

        private void InitBullets(Collider bulletCollider)
        {
            var entity = _system.GetWorld().NewEntity();
            ref var bulletData = ref _bulletData.Value.Add(entity);
            var bulletSettings = bulletCollider.GetComponent<BulletSettings>();
            bulletData.BulletRigidBody = bulletCollider.GetComponent<Rigidbody>();
            bulletData.BulletCollider = bulletCollider;
            bulletData.DefaultLifeTime = bulletSettings.DefaultLifeTime;
            bulletData.Damage = bulletSettings.Damage;
            
            ref var unityPhysicsCollisionDataComponent = ref _unityPhysicsCollisionDataComponent.Value.Add(entity);
            unityPhysicsCollisionDataComponent.CollisionsEnter = new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();
            bulletData.Detector = bulletSettings.GetComponent<UnityPhysicsCollisionDetector>();
            bulletData.Detector.Init(entity, _system.GetWorld());
        }
    }
}