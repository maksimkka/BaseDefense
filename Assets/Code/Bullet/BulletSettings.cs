using UnityEngine;

namespace Code.Bullet
{
    [DisallowMultipleComponent]
    public class BulletSettings : MonoBehaviour
    {
        [field: SerializeField] public float DefaultLifeTime { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
    }
}