using UnityEngine;

namespace Code.Bullet
{
    public struct c_BulletData
    {
        public Collider BulletGameObject;
        public Rigidbody BulletRigidBody;
        public float DefaultLifeTime;
        public float CurrentLifeTime;
    }
}