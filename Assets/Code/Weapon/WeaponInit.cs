using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Weapon
{
    public sealed class WeaponInit : IEcsInitSystem
    {
        private readonly EcsPoolInject<WeaponData> _weaponData = default;
        private readonly EcsCustomInject<WeaponSettings> _weaponSettings = default;
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var heroShootData = ref _weaponData.Value.Add(entity);
            heroShootData.WeaponParent = _weaponSettings.Value.gameObject.transform;
            heroShootData.ShootForce = _weaponSettings.Value.ShootForce;
            heroShootData.StartPoolSize = _weaponSettings.Value.StartPoolSize;
            heroShootData.ReloadTime = _weaponSettings.Value.ShootCooldown;
            heroShootData.BulletPrefab = _weaponSettings.Value.BulletPrefab;
        }
    }
}