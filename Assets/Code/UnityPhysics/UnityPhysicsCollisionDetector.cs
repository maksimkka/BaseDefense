using Leopotam.EcsLite;
using UnityEngine;

namespace Code.UnityPhysics
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class UnityPhysicsCollisionDetector : MonoBehaviour
    {
        private int Entity { get; set; }
        private EcsWorld _world;
        private Collider _collider;

        public void Init(int entity, EcsWorld world)
        {
            Entity = entity;
            _world = world;
            _collider = GetComponent<Collider>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.collider.TryGetComponent(out UnityPhysicsCollisionDetector otherDetector)) return;

            var contacts = collision.contacts[0];

            var dto = new UnityPhysicsCollisionDTO(Entity, _collider, otherDetector.Entity, collision.collider,
                contacts.point);
            
            ref var collisionData = ref _world.GetPool<UnityPhysicsCollisionDataComponent>().Get(Entity);
            collisionData.CollisionsEnter.Enqueue((collision.gameObject.layer, dto));
        }
    }
}