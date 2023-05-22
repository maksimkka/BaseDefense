using System.Collections.Generic;
using Code.Bullet;
using Code.Pools;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Weapon
{
    public sealed class i_Bullet : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<c_WeaponData>> _weaponFilter = default;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> UnityPhysicsCollisionDataComponent;
        private readonly EcsPoolInject<c_BulletData> c_BulletData = default;

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
            bulletSettings.SetEntity(entity);
            bulletData.BulletRigidBody = bulletCollider.GetComponent<Rigidbody>();
            bulletData.BulletGameObject = bulletCollider;
            bulletData.DefaultLifeTime = bulletSettings.DefaultLifeTime;
            bulletData.Damage = bulletSettings.Damage;
            
            ref var unityPhysicsCollisionDataComponent = ref UnityPhysicsCollisionDataComponent.Value.Add(entity);
            unityPhysicsCollisionDataComponent.CollisionsEnter = new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();
            bulletData.Detector = bulletSettings.GetComponent<UnityPhysicsCollisionDetector>();
            bulletData.Detector.Init(entity, _system.GetWorld());
        }
    }
}