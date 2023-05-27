using UnityEngine;

namespace Code.Enemy
{
    [DisallowMultipleComponent]
    public class EnemySettings : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float DetectionDistance { get; private set; }
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField] public float CooldownAttack { get; private set; }
        [field: SerializeField] public int HP { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
    }
}