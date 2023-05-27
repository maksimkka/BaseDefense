using Code.Enemy;
using Code.Pools;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Bullet
{
    public class BulletCollision : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BulletData, UnityPhysicsCollisionDataComponent>, Exc<ReturnsToPoolRequest>> _bulletFilter = default;
        private readonly EcsPoolInject<ReturnsToPoolRequest> _returnsToPoolRequest = default;
        private readonly EcsPoolInject<TakingDamageRequest> _takingDamageRequest = default;
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
                    _returnsToPoolRequest.Value.Add(bulletDataEntity); 
                    ref var takingDamage = ref _takingDamageRequest.Value.Add(enemyDataEntity);
                    takingDamage.Damage = bulletData.Damage;
                }
            }
            
            collisionData.CollisionsEnter.Clear();
        }
    }
}