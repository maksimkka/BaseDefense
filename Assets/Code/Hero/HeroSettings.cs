using UnityEngine;

namespace Code.Hero
{
    [DisallowMultipleComponent]
    public sealed class HeroSettings : MonoBehaviour
    {
        [field: SerializeField] public Transform HeroStartPosition { get; private set; }
        [field: SerializeField] public GameObject HealthPosition { get; private set; }
        [field: SerializeField] public float BonusSearchRadius { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int HP { get; private set; }
        [field: SerializeField] public int Distance { get; private set; }
        [field: SerializeField] public float RegenDelay { get; private set; }
        [field: SerializeField] public int RegenRate { get; private set; }
        
    }
}