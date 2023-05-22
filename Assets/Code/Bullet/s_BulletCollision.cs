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
        private readonly EcsPoolInject<r_TakingDamage> r_TakingDamage = default;
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
                var enemyDataEntity = collisionEnter.dto.OtherEntity;
                ref var bulletData = ref _bulletFilter.Pools.Inc1.Get(bulletDataEntity);

                if (collisionEnter.dto.OtherCollider.gameObject.layer == Layers.Enemy)
                {
                    "1111111111111111".Colored(Color.cyan).Log();
                    r_ReturnsToThePool.Value.Add(bulletDataEntity); 
                    ref var takingDamage = ref r_TakingDamage.Value.Add(enemyDataEntity);
                    takingDamage.Damage = bulletData.Damage;
                }
            }
            
            collisionData.CollisionsEnter.Clear();
        }
    }
}