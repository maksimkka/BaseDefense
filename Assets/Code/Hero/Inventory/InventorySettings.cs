using UnityEngine;

namespace Code.Hero
{
    [DisallowMultipleComponent]
    public class InventorySettings : MonoBehaviour
    {
        //public Transform spawnPoint;
        [field: SerializeField] public int MaxStackSize { get; private set; }
        [field: SerializeField] public float OffsetPosition { get; private set; }
    }
}