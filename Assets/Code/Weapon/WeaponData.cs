using Code.Pools;
using UnityEngine;

namespace Code.Weapon
{
    public struct WeaponData
    {
        public ObjectPool<Collider> BulletsPool;
        public GameObject BulletPrefab;
        public Transform WeaponParent;
        public int StartPoolSize;
        public float ReloadTime;
        public float CurrentReloadTime;
        public float ShootForce;
    }
}