using System.Collections.Generic;
using UnityEngine;

namespace Code.Hero
{
    public struct InventoryData
    {
        public List<GameObject> StackInventory;
        public Transform InventoryObject;
        public Transform CurrentPosition;
        public int MaxStackSize;
        public float OffsetPosition;
    }
}