using UnityEngine;

namespace Code.Weapon
{
    [DisallowMultipleComponent]
    public class WeaponSettings : MonoBehaviour
    {
        [field: SerializeField] public GameObject BulletPrefab { get; private set; }
        [field: SerializeField] public float ShootCooldown { get; private set; }
        [field: SerializeField] public float ShootForce { get; private set; }
        [field: SerializeField] public int StartPoolSize { get; private set; }
    }
}