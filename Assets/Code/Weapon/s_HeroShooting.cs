using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Weapon
{
    public sealed class s_HeroShooting : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_WeaponData>> _weaponFilter = default;

        private float _currentReloadTime;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _weaponFilter.Value)
            {
                ref var weapon = ref _weaponFilter.Pools.Inc1.Get(entity);
                Reload(ref weapon);
            }
        }

        private void Reload(ref c_WeaponData weapon)
        {
            _currentReloadTime += Time.deltaTime;
            if (_currentReloadTime >= weapon.ReloadTime)
            {
                _currentReloadTime = 0;
                Shoot(ref weapon);
            }
        }
        
        private void Shoot(ref c_WeaponData weapon)
        {
            var BulletSpawnPosition = weapon.BulletSpawnPosition.localPosition;
            var bullet = weapon.BulletsPool.GetObject(BulletSpawnPosition, Quaternion.identity);
            var rigidBody = bullet.GetComponent<Rigidbody>();
            rigidBody.AddForce(Vector3.forward * weapon.ShootForce, ForceMode.Impulse);
        }
    }
}