using Code.Pools;
using UnityEngine;

namespace Code.Weapon
{
    public struct c_WeaponData
    {
        public ObjectPool<Collider> BulletsPool;
        public GameObject BulletPrefab;
        public Transform BulletSpawnPosition;
        public int StartPoolSize;
        public float ReloadTime;
        public float ShootForce;
    }
}