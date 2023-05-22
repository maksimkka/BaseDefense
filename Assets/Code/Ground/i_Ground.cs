using System.Collections.Generic;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Ground
{
    public sealed class i_Ground : IEcsInitSystem
    {
        private readonly EcsPoolInject<c_GroundData> c_GroundData;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> UnityPhysicsCollisionDataComponent;
        private readonly EcsCustomInject<GroundSettings[]> _groundSettings;
        public void Init(IEcsSystems systems)
        {
            foreach (var groundSettings in _groundSettings.Value)
            {
                var entity = systems.GetWorld().NewEntity();
                ref var groundData = ref c_GroundData.Value.Add(entity);
                ref var unityPhysicsCollisionDataComponent = ref UnityPhysicsCollisionDataComponent.Value.Add(entity);
                unityPhysicsCollisionDataComponent.CollisionsEnter = new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();
                groundData.Detector = groundSettings.GetComponent<UnityPhysicsCollisionDetector>();
                groundData.Detector.Init(entity, systems.GetWorld());
            }
        }
    }
}