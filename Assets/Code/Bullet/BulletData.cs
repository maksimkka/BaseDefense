using Code.UnityPhysics;
using UnityEngine;

namespace Code.Bullet
{
    public struct BulletData
    {
        public UnityPhysicsCollisionDetector Detector;
        public Collider BulletCollider;
        public Rigidbody BulletRigidBody;
        public float DefaultLifeTime;
        public float CurrentLifeTime;
        public int Damage;
    }
}