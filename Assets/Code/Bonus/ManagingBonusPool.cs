using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Bonus
{
    public class ManagingBonusPool : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BonusSpawnerData, SpawnBonusRequest>> _bonusSpawnerDataFilter = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _bonusSpawnerDataFilter.Value)
            {
                ref var bonusSpawner = ref _bonusSpawnerDataFilter.Pools.Inc1.Get(entity);
                ref var spawnRequest = ref _bonusSpawnerDataFilter.Pools.Inc2.Get(entity);
                RandomSpawn(ref bonusSpawner, spawnRequest.SpawnPosition);
                _bonusSpawnerDataFilter.Pools.Inc2.Del(entity);
            }
        }

        private void RandomSpawn(ref BonusSpawnerData bonusSpawner, Vector3 spawnPosition)
        {
            var randomNumber = Random.value;

            if (randomNumber < bonusSpawner.SpawnProbabilityRegularBonus)
            {
                for (int i = 0; i < 3; i++)
                {
                   var bonus= bonusSpawner.RegularBonusPool.GetObject(spawnPosition, Quaternion.identity);
                   var rigidBody = bonus.GetComponent<Rigidbody>();
                   rigidBody.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), 1.5f, Random.Range(-0.5f, 0.5f)) * 2,
                       ForceMode.Impulse);
                   rigidBody.AddRelativeTorque(
                       new Vector3(Random.Range(-15, 15), 10, Random.Range(-5, 5)) * 2,
                       ForceMode.Impulse);
                }
            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    var bonus = bonusSpawner.MegaBonusPool.GetObject(spawnPosition, Quaternion.identity);
                    var rigidBody = bonus.GetComponent<Rigidbody>();

                    rigidBody.AddForce(new Vector3(Random.Range(-0.5f, 0.5f), 1.5f, Random.Range(-0.5f, 0.5f)) * 2,
                        ForceMode.Impulse);
                    rigidBody.AddRelativeTorque(
                        new Vector3(Random.Range(-15, 15), 10, Random.Range(-5, 5)) * 2,
                        ForceMode.Impulse);
                }
            }
        }

        private void Spawn(BonusSpawnerData BonusSpawnerData, int quantitySpawnObjects)
        {
            for (int i = 0; i < quantitySpawnObjects; i++)
            {
                //BonusSpawnerData.MegaBonusPool.GetObject()
            }
        }
    }
}