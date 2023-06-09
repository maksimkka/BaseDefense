﻿using UnityEngine;

namespace Code.Bonus
{
    public struct BonusData
    {
        public GameObject BonusGameObject;
        public Rigidbody BonusRigidbody;
        public BonusType BonusType;
        public int BonusValue;
        public float ForceDirectionDiapason;
        public float ForceRotationDiapason;
        public bool IsPickUp;
    }
}