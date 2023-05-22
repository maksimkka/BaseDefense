using UnityEngine;

namespace Code.Hero
{
    [DisallowMultipleComponent]
    public sealed class HeroSettings : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public int HP { get; private set; }
        [field: SerializeField] public int Distance { get; private set; }
    }
}