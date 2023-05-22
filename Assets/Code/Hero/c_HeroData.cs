using Code.UnityPhysics;
using UnityEngine;

namespace Code.Hero
{
    public struct c_HeroData
    {
        public GameObject HeroGameObject;
        public Rigidbody heroRigidBody;
        public Transform TargetRotation;
        public float Distance;
        public float Speed;
        public int HP;
    }
}