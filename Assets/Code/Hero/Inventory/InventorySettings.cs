using UnityEngine;

namespace Code.Hero.Inventory
{
    [DisallowMultipleComponent]
    public class InventorySettings : MonoBehaviour
    {
        [field: SerializeField] public int MaxStackSize { get; private set; }
        [field: SerializeField] public float OffsetPosition { get; private set; }
    }
}