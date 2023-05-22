using UnityEngine;

namespace Code.Enemy
{
    [DisallowMultipleComponent]
    public sealed class EnemySettings : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float Distance { get; private set; }
        [field: SerializeField] public float CooldownAttack { get; private set; }
        [field: SerializeField] public int HP { get; private set; }
    }
}