using System.Collections.Generic;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Ground
{
    public class GroundInit : IEcsInitSystem
    {
        private readonly EcsPoolInject<GroundData> _groundData = default;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> _unityPhysicsCollisionDataComponent = default;
        private readonly EcsCustomInject<GroundMarker[]> _groundSettings = default;
        public void Init(IEcsSystems systems)
        {
            foreach (var groundSettings in _groundSettings.Value)
            {
                var entity = systems.GetWorld().NewEntity();
                ref var groundData = ref _groundData.Value.Add(entity);
                ref var unityPhysicsCollisionDataComponent = ref _unityPhysicsCollisionDataComponent.Value.Add(entity);
                unityPhysicsCollisionDataComponent.CollisionsEnter = new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();
                groundData.Detector = groundSettings.GetComponent<UnityPhysicsCollisionDetector>();
                groundData.Detector.Init(entity, systems.GetWorld());
            }
        }
    }
}