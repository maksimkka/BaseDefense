using Code.Ground;
using Code.Logger;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public sealed class s_GroundChecker : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_CurrentGroundData, UnityPhysicsCollisionDataComponent>> _groundCollisionFilter = default;
        private readonly EcsPoolInject<r_ChangeGround> r_ChangeGround;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _groundCollisionFilter.Value)
            {
                CheckGround(entity);
            }
        }

        private void CheckGround(int entity)
        {
            ref var collisionData = ref _groundCollisionFilter.Pools.Inc2.Get(entity);
            foreach (var collisionEnter in collisionData.CollisionsEnter)
            {
                var groundDataEntity = collisionEnter.dto.SelfEntity;
                ref var groundData = ref _groundCollisionFilter.Pools.Inc1.Get(groundDataEntity);
                if (collisionEnter.dto.OtherCollider.gameObject.layer == Layers.BaseGround)
                {
                    groundData.IsBaseGround = true;
                    r_ChangeGround.Value.Add(groundDataEntity);
                    $"BASE COLLISION".Colored(Color.cyan).Log();
                }

                else if (collisionEnter.dto.OtherCollider.gameObject.layer == Layers.OtherGround)
                {
                    groundData.IsBaseGround = false;
                    r_ChangeGround.Value.Add(groundDataEntity);
                    $"GROUND COLLISION".Colored(Color.yellow).Log();
                }
            }

            collisionData.CollisionsEnter.Clear();
        }
    }
}