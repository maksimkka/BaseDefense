using Code.Pools;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bonus
{
    public class BonusSpawnerInit : IEcsInitSystem
    {
        private readonly EcsPoolInject<BonusSpawnerData> _bonusSpawnerData = default;
        private readonly EcsCustomInject<BonusesPoolSettings> _bonusesPoolSettings = default;
        private readonly EcsPoolInject<BonusData> _bonusData = default;
        private IEcsSystems _system;
        public void Init(IEcsSystems systems)
        {
            _system = systems;
            var entity = systems.GetWorld().NewEntity();
            ref var bonusSpawner = ref _bonusSpawnerData.Value.Add(entity);

            bonusSpawner.SpawnerParentObject = _bonusesPoolSettings.Value.gameObject;
            bonusSpawner.MegaBonusPrefab = _bonusesPoolSettings.Value.MegaBonusPrefab;
            bonusSpawner.RegularBonusPrefab = _bonusesPoolSettings.Value.RegularBonusPrefab;
            bonusSpawner.StartSizeMegaBonusPool = _bonusesPoolSettings.Value.StartSizeMegaBonusPool;
            bonusSpawner.StartSizeRegularBonusPool = _bonusesPoolSettings.Value.StartSizeRegularBonusPool;
            bonusSpawner.SpawnProbabilityRegularBonus = _bonusesPoolSettings.Value.SpawnProbabilityRegularBonus;

            var regularBonusCollider = bonusSpawner.RegularBonusPrefab.GetComponent<Collider>();
            var megaBonusCollider = bonusSpawner.MegaBonusPrefab.GetComponent<Collider>();
            
            var regularBonusesPool = new ObjectPool<Collider>(regularBonusCollider,
                bonusSpawner.SpawnerParentObject.transform, bonusSpawner.StartSizeRegularBonusPool, 
                initWithInEcs: InitBonuses);   
            
            var megaBonusesPool = new ObjectPool<Collider>(megaBonusCollider,
                bonusSpawner.SpawnerParentObject.transform, bonusSpawner.StartSizeMegaBonusPool, 
                initWithInEcs: InitBonuses);

            bonusSpawner.MegaBonusPool = megaBonusesPool;
            bonusSpawner.RegularBonusPool = regularBonusesPool;
        }

        private void InitBonuses(Collider collider)
        {
            var entity = _system.GetWorld().NewEntity();
            var bonusSettings = collider.GetComponent<BonusSettings>();
            ref var bonusData = ref _bonusData.Value.Add(entity);

            bonusData.BonusType = bonusSettings.BonusType;
            bonusData.BonusGameObject = bonusSettings.gameObject;
            bonusData.BonusRigidbody = bonusSettings.gameObject.GetComponent<Rigidbody>();
            bonusData.BonusValue = bonusSettings.BonusValue;
            bonusData.ForceDirectionDiapason = bonusSettings.ForceDirectionDiapason;
            bonusData.ForceRotationDiapason = bonusSettings.ForceRotationDiapason;
        }
    }
}