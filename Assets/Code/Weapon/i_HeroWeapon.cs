using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Weapon
{
    public sealed class i_HeroWeapon : IEcsInitSystem
    {
        private readonly EcsPoolInject<c_WeaponData> c_HeroShootData = default;
        private readonly EcsCustomInject<WeaponSettings> _weaponSettings = default;
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var heroShootData = ref c_HeroShootData.Value.Add(entity);
            heroShootData.BulletSpawnPosition = _weaponSettings.Value.gameObject.transform;
            heroShootData.ShootForce = _weaponSettings.Value.ShootForce;
            heroShootData.StartPoolSize = _weaponSettings.Value.StartPoolSize;
            heroShootData.ReloadTime = _weaponSettings.Value.ShootCooldown;
            heroShootData.BulletPrefab = _weaponSettings.Value.BulletPrefab;
        }
    }
}