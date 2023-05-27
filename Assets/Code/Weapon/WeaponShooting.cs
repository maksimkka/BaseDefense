using Code.Bullet;
using Code.EndGame;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Weapon
{
    public class WeaponShooting : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<WeaponData, CanShootMarker>, Exc<EndGameMarker>> _weaponFilter = default;
        private readonly EcsFilterInject<Inc<BulletData>> _bulletDataFilter = default;
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _weaponFilter.Value)
            {
                ref var weapon = ref _weaponFilter.Pools.Inc1.Get(entity);
                Reload(ref weapon);
            }
        }

        private void Reload(ref WeaponData weapon)
        {
            weapon.CurrentReloadTime += Time.deltaTime;
            if (weapon.CurrentReloadTime >= weapon.ReloadTime)
            {
                weapon.CurrentReloadTime = 0;
                Shoot(ref weapon);
            }
        }

        private void Shoot(ref WeaponData weapon)
        {
            var bullet = GetBullet(ref weapon);
            
            foreach (var entity in _bulletDataFilter.Value)
            {
                ref var bulletData = ref _bulletDataFilter.Pools.Inc1.Get(entity);
                if(bulletData.BulletCollider.gameObject.GetHashCode() != bullet.GetHashCode()) continue;
                bulletData.BulletRigidBody.AddForce(weapon.WeaponParent.forward * weapon.ShootForce, ForceMode.Impulse);
            }
        }

        private GameObject GetBullet(ref WeaponData weapon)
        {
            var BulletSpawnPosition = weapon.WeaponParent.position;
            var bullet = weapon.BulletsPool.GetObject(BulletSpawnPosition, Quaternion.identity);
            return bullet.gameObject;
        }
    }
}