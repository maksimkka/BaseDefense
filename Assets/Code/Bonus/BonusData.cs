using UnityEngine;

namespace Code.Bonus
{
    public struct BonusData
    {
        public GameObject BonusGameObject;
        public Rigidbody BonusRigidbody;
        public BonusType BonusType;
        public int BonusValue;
        public bool IsPickUp;
    }
}