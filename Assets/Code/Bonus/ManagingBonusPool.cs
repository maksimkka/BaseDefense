using Code.Pools;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bonus
{
    public class ManagingBonusPool : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BonusSpawnerData, SpawnBonusRequest>> _bonusSpawnFilter = default;
        private readonly EcsFilterInject<Inc<BonusData>> _bonusDataFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _bonusSpawnFilter.Value)
            {
                ref var bonusSpawner = ref _bonusSpawnFilter.Pools.Inc1.Get(entity);
                ref var spawnRequest = ref _bonusSpawnFilter.Pools.Inc2.Get(entity);
                RandomSpawn(ref bonusSpawner, spawnRequest.SpawnPosition);
                _bonusSpawnFilter.Pools.Inc2.Del(entity);
            }
        }

        private void RandomSpawn(ref BonusSpawnerData bonusSpawner, Vector3 spawnPosition)
        {
            var randomNumber = Random.value;

            if (randomNumber < bonusSpawner.SpawnProbabilityRegularBonus)
            {
                GetPoolObjects(bonusSpawner.RegularBonusPool, spawnPosition, 3);
            }

            else
            {
                GetPoolObjects(bonusSpawner.MegaBonusPool, spawnPosition, 1);
            }
        }

        private void GetPoolObjects(ObjectPool<Collider> pool, Vector3 spawnPosition, int countSpawnObject)
        {
            for (int i = 0; i < countSpawnObject; i++)
            {
                var bonus = pool.GetObject(spawnPosition, Quaternion.identity);
                ThrowUp(bonus.gameObject);
            }
        }

        private void ThrowUp(GameObject bonus)
        {
            var bonusGameObject = bonus;

            foreach (var entity in _bonusDataFilter.Value)
            {
                ref var bonusData = ref _bonusDataFilter.Pools.Inc1.Get(entity);
                if (bonusData.BonusGameObject.gameObject.GetHashCode() != bonusGameObject.GetHashCode()) continue;
                
                bonusData.BonusRigidbody.AddForce(new Vector3(RandomValue(bonusData.ForceDirectionDiapason),
                        bonusData.ForceDirectionDiapason, RandomValue(bonusData.ForceDirectionDiapason)),
                    ForceMode.Impulse);

                bonusData.BonusRigidbody.AddRelativeTorque(new Vector3(RandomValue(bonusData.ForceRotationDiapason),
                        bonusData.ForceRotationDiapason, RandomValue(bonusData.ForceRotationDiapason)),
                    ForceMode.Impulse);

                return;
            }
        }

        private float RandomValue(float diapasonValue)
        {
            return Random.Range(-diapasonValue, diapasonValue);
        }
    }
}