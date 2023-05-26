using System;
using System.Collections.Generic;
using Code.Bonus;
using Code.Logger;
using Code.Score;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public class InventoryCleaning : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<InventoryData, ClearInventoryRequest>> _InventoryDataFilter = default;
        private readonly EcsFilterInject<Inc<BonusSpawnerData>> _bonusSpawnerDataFilter = default;
        private readonly EcsFilterInject<Inc<BonusData>> _bonusData = default;
        private readonly EcsFilterInject<Inc<ScoreData>> _scoreData = default;
        private readonly EcsPoolInject<ScoreChangeRequest> _scoreChangeRequest = default;
        public void Run(IEcsSystems systems)
        {
            ClearInventory();
        }
        private void ClearInventory()
        {
            foreach (var currentGroundEntity in _InventoryDataFilter.Value)
            {
                ref var inventoryData = ref _InventoryDataFilter.Pools.Inc1.Get(currentGroundEntity);
                ref var clearInventory = ref _InventoryDataFilter.Pools.Inc2.Get(currentGroundEntity);
                inventoryData.CurrentOffsetPosition = inventoryData.DefaultOffsetPosition;
                ReturnToPool(inventoryData.BonusEntities, clearInventory.IsRestart);

                _InventoryDataFilter.Pools.Inc2.Del(currentGroundEntity);
            }
        }

        private void ReturnToPool(List<int> bonusEntities, bool isRestart)
        {
            foreach (var entity in _bonusSpawnerDataFilter.Value)
            {
                ref var BonusSpawnerData = ref _bonusSpawnerDataFilter.Pools.Inc1.Get(entity);

                foreach (var bonus in bonusEntities)
                {
                    ref var bonusData = ref _bonusData.Pools.Inc1.Get(bonus);
                    if (bonusData.BonusType == BonusType.RegularBonus)
                    {
                        var bonusCollider = bonusData.BonusGameObject.GetComponent<Collider>();
                        BonusSpawnerData.RegularBonusPool.ReturnObject(bonusCollider,
                            BonusSpawnerData.SpawnerParentObject.transform);
                    }

                    else
                    {
                        var bonusCollider = bonusData.BonusGameObject.GetComponent<Collider>();
                        BonusSpawnerData.MegaBonusPool.ReturnObject(bonusCollider,
                            BonusSpawnerData.SpawnerParentObject.transform);
                    }
                }
            }
            
            if (!isRestart)
            {
                AddChangeScoreRequest(bonusEntities);
            }

            bonusEntities.Clear();
            ChangeBonusesSettings();
        }

        private void ChangeBonusesSettings()
        {
            foreach (var entity in _bonusData.Value)
            {
                ref var bonus = ref _bonusData.Pools.Inc1.Get(entity);
                if (!bonus.IsPickUp) continue;

                bonus.BonusRigidbody.isKinematic = false;
                bonus.IsPickUp = false;
            }
        }

        private void AddChangeScoreRequest(List<int> bonusEntities)
        {
            foreach (var entity in _scoreData.Value)
            {
                if(_scoreChangeRequest.Value.Has(entity)) return;
                ref var scoreChange = ref _scoreChangeRequest.Value.Add(entity);
                scoreChange.StackInventory = new List<int>(bonusEntities);
            }
        }
    }
}