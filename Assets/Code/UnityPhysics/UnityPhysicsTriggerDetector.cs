using Leopotam.EcsLite;
using UnityEngine;

namespace Code.UnityPhysics
{
    public sealed class UnityPhysicsTriggerDetector : MonoBehaviour
    {
        public int Entity { get; private set; }
        private EcsWorld _world;
        private Collider _collider;

        public void Init(int entity, EcsWorld world)
        {
            Entity = entity;
            _world = world;
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherDetector = other.GetComponent<UnityPhysicsTriggerDetector>();
            var contacts = other.transform.position;
            var dto = new UnityPhysicsCollisionDTO(Entity, _collider, otherDetector.Entity, other, contacts);

            ref var collisionData = ref _world.GetPool<UnityPhysicsCollisionDataComponent>().Get(Entity);
            collisionData.CollisionsEnter.Enqueue((other.gameObject.layer, dto));
        }

        private void OnTriggerStay(Collider other)
        {
            var otherDetector = other.GetComponent<UnityPhysicsTriggerDetector>();
            var contacts = other.transform.position;
            var dto = new UnityPhysicsCollisionDTO(Entity, _collider, otherDetector.Entity, other, contacts);

            ref var collisionData = ref _world.GetPool<UnityPhysicsCollisionDataComponent>().Get(Entity);
            var collisionStay = collisionData.CollisionsStay;
            if (!collisionStay.ContainsKey(other.gameObject))
            {
                collisionStay.Add(other.gameObject, dto);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            ref var collisionData = ref _world.GetPool<UnityPhysicsCollisionDataComponent>().Get(Entity);
            var collisionStay = collisionData.CollisionsStay;
            if (collisionStay.ContainsKey(other.gameObject)) collisionStay.Remove(other.gameObject);
        }
    }
}