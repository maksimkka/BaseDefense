using Code.Enemy;
using Code.Logger;
using Code.Pools;
using Code.UnityPhysics;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bullet
{
    public class s_BulletCollision : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_BulletData, UnityPhysicsCollisionDataComponent>, Exc<r_ReturnsToThePool>> _bulletFilter = default;
        private readonly EcsPoolInject<c_Enemy> c_Enemy = default;
        private readonly EcsPoolInject<r_ReturnsToThePool> r_ReturnsToThePool = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _bulletFilter.Value)
            {
                CheckCollision(entity);
            }
        }

        private void CheckCollision(int entity)
        {
            ref var collisionData = ref _bulletFilter.Pools.Inc2.Get(entity);
            foreach (var collisionEnter in collisionData.CollisionsEnter)
            {
                var bulletDataEntity = collisionEnter.dto.SelfEntity;
                ref var bulletData = ref _bulletFilter.Pools.Inc1.Get(bulletDataEntity);

                if (collisionEnter.dto.OtherCollider.gameObject.layer == Layers.Enemy)
                {
                    r_ReturnsToThePool.Value.Add(bulletDataEntity);
                }
            }
        }
    }
}