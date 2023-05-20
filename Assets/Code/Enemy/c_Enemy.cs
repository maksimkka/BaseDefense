using UnityEngine;
using UnityEngine.AI;

namespace Code.Enemy
{
    public struct c_Enemy
    {
        public GameObject EnemyGameObject;
        public NavMeshAgent NavMeshAgent;
        public GameObject TargetMove;
        public EnemyStates States;
        public Rigidbody EnemyRigidBody;
        public float CooldownAttack;
        public float Distance;
        public float Speed;
        public int HP;
        
    }
}