using UnityEngine;
using UnityEngine.UI;

namespace Code.Hero
{
    public struct c_HeroData
    {
        public AnimationSwitcher Animator;
        public GameObject HeroGameObject;
        public GameObject HealthPosition;
        public Rigidbody HeroRigidBody;
        public Transform TargetRotation;
        public Transform StartPosition;
        public Slider Slider;

        public float Distance;
        public float Speed;
        public int DefaultHP;
        public int CurrentHP;
        
        public int IdleAnimationHash;
        public int RunAnimationHash;
        public int RiffleWalkAnimationHash;
        public int RiffleIdleAnimation;
    }
}