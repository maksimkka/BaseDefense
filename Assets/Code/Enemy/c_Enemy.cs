using Code.UnityPhysics;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    public struct c_Enemy
    {
        public UnityPhysicsCollisionDetector Detector;
        public Collider EnemyGameObject;
        public NavMeshAgent NavMeshAgent;
        public GameObject TargetMove;
        public EnemyStates States;
        public float CooldownAttack;
        public float Distance;
        public float Speed;
        public int DefaultHP;
        public int CurrentHP;
        
    }
}