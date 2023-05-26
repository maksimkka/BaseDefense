using Code.Pools;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bonus
{
    public class ManagingBonusPool : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BonusSpawnerData, SpawnBonusRequest>> _bonusSpawnFilter = default;
        // private readonly EcsFilterInject<Inc<BonusSpawnerData>> _BonusSpawnerDataFilter = default;
        // private readonly EcsFilterInject<Inc<BonusData>> _bonusData = default;
        // private readonly EcsFilterInject<Inc<InventoryData, BroughtBonusesToBaseRequest>> _InventoryDataFilter = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _bonusSpawnFilter.Value)
            {
                ref var bonusSpawner = ref _bonusSpawnFilter.Pools.Inc1.Get(entity);
                ref var spawnRequest = ref _bonusSpawnFilter.Pools.Inc2.Get(entity);
                RandomSpawn(ref bonusSpawner, spawnRequest.SpawnPosition);
                _bonusSpawnFilter.Pools.Inc2.Del(entity);
            }

            // ChangeStateEnemies();
        }

        private void RandomSpawn(ref BonusSpawnerData bonusSpawner, Vector3 spawnPosition)
        {
            var randomNumber = Random.value;

            if (randomNumber < bonusSpawner.SpawnProbabilityRegularBonus)
            {
                GetAndThrowUpPoolObjects(bonusSpawner.RegularBonusPool, spawnPosition, 3);
            }

            else
            {
                GetAndThrowUpPoolObjects(bonusSpawner.MegaBonusPool, spawnPosition, 1);
            }
        }

        private void GetAndThrowUpPoolObjects(ObjectPool<Collider> pool, Vector3 spawnPosition, int countSpawnObject)
        {
            for (int i = 0; i < countSpawnObject; i++)
            {
                var bonus = pool.GetObject(spawnPosition, Quaternion.identity);
                var rigidBody = bonus.GetComponent<Rigidbody>();

                rigidBody.AddForce(new Vector3(Random.Range(-1f, 1f), 1.5f, Random.Range(-1f, 1f)),
                    ForceMode.Impulse);
                rigidBody.AddRelativeTorque(
                    new Vector3(Random.Range(-15, 15), 10, Random.Range(-5, 5)) * 2,
                    ForceMode.Impulse);
            }
        }

        // private void ChangeStateEnemies()
        // {
        //     foreach (var currentGroundEntity in _InventoryDataFilter.Value)
        //     {
        //         ref var inventoryData = ref _InventoryDataFilter.Pools.Inc1.Get(currentGroundEntity);
        //         inventoryData.CurrentOffsetPosition = inventoryData.DefaultOffsetPosition;
        //         ReturnToPool(ref inventoryData.StackInventory);
        //
        //         _InventoryDataFilter.Pools.Inc2.Del(currentGroundEntity);
        //     }
        // }
        //
        // private void ReturnToPool(ref List<BonusSettings> bonuses)
        // {
        //     foreach (var entity in _BonusSpawnerDataFilter.Value)
        //     {
        //         ref var BonusSpawnerData = ref _BonusSpawnerDataFilter.Pools.Inc1.Get(entity);
        //
        //         foreach (var bonus in bonuses)
        //         {
        //             if (bonus.BonusType == BonusType.RegularBonus)
        //             {
        //                 var bonusCollider = bonus.GetComponent<Collider>();
        //                 BonusSpawnerData.RegularBonusPool.ReturnObject(bonusCollider,
        //                     BonusSpawnerData.SpawnerParentObject.transform);
        //             }
        //
        //             else
        //             {
        //                 var bonusCollider = bonus.GetComponent<Collider>();
        //                 BonusSpawnerData.MegaBonusPool.ReturnObject(bonusCollider,
        //                     BonusSpawnerData.SpawnerParentObject.transform);
        //             }
        //         }
        //     }
        //     
        //     bonuses.Clear();
        //     ChangeBonusesSettings();
        // }
        //
        // private void ChangeBonusesSettings()
        // {
        //     foreach (var entity in _bonusData.Value)
        //     {
        //         ref var bonus = ref _bonusData.Pools.Inc1.Get(entity);
        //         if (!bonus.IsPickUp) continue;
        //
        //         bonus.BonusRigidbody.isKinematic = false;
        //         bonus.IsPickUp = false;
        //     }
        // }
    }
}