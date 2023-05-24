using Code.UnityPhysics;
using UnityEngine;

namespace Code.Hero
{
    public struct c_HeroData
    {
        public HeroAnimation HeroAnimator;
        public GameObject HeroGameObject;
        public Rigidbody HeroRigidBody;
        public Transform TargetRotation;
        public float Distance;
        public float Speed;
        public int HP;
        
        public int IdleAnimationHash;
        public int RunAnimationHash;
        public int RiffleWalkAnimationHash;
        public int RiffleIdleAnimation;
    }
}