using UnityEngine;

namespace Code.Hero
{
    [DisallowMultipleComponent]
    public sealed class HeroSettings : MonoBehaviour
    {
        [field: SerializeField] public float Speed { get; private set; }
    }
}