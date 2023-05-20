using UnityEngine;

namespace Code.UnityPhysics
{
    public readonly struct UnityPhysicsCollisionDTO
    {
        public readonly int SelfEntity;
        public readonly Collider SelfCollider;
        public readonly int OtherEntity;
        public readonly Collider OtherCollider;
        public readonly Vector3 CollisionPoint;

        public UnityPhysicsCollisionDTO(int selfEntity, Collider selfCollider, int otherEntity, Collider otherCollider, Vector3 collisionPoint)
        {
            SelfEntity = selfEntity;
            SelfCollider = selfCollider;
            OtherEntity = otherEntity;
            OtherCollider = otherCollider;
            CollisionPoint = collisionPoint;
        }
    }
}