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
        public bool IsReadyAttack;
        public float DefaultReloadTime;
        public float CurrentReloadTime;
        public float CurrentDistance;
        public float DetectionDistance;
        public float AttackDistance;
        public float Speed;
        public int DefaultHP;
        public int CurrentHP;
        
    }
}