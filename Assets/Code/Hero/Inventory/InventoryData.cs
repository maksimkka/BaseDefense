using System.Collections.Generic;
using UnityEngine;

namespace Code.Hero.Inventory
{
    public struct InventoryData
    {
        public List<int> BonusEntities;
        public Transform InventoryObject;
        public Transform CurrentPosition;
        public int MaxStackSize;
        public float CurrentOffsetPosition;
        public float DefaultOffsetPosition;
    }
}