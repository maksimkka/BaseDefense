using Code.UnityPhysics;
using UnityEngine;

namespace Code.Bullet
{
    public struct c_BulletData
    {
        public UnityPhysicsCollisionDetector Detector;
        public Collider BulletGameObject;
        public Rigidbody BulletRigidBody;
        public float DefaultLifeTime;
        public float CurrentLifeTime;
        public int Damage;
    }
}