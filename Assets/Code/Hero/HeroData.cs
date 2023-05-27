using Code.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Hero
{
    public struct HeroData
    {
        public AnimationSwitcher Animator;
        public GameObject HeroGameObject;
        public GameObject HealthPosition;
        public Rigidbody HeroRigidBody;
        public Transform TargetRotation;
        public Transform StartPosition;
        public Slider Slider;

        public float DetectionDistance;
        public float BonusSearchRadius;
        public float Speed;
        public float RegenHPTimer;
        public float RegenDelay;
        public int RegenRate;
        public int DefaultHP;
        public int CurrentHP;
        
        public int IdleAnimationHash;
        public int RunAnimationHash;
        public int RiffleWalkAnimationHash;
        public int RiffleIdleAnimation;
    }
}