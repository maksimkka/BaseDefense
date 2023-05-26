using Code.Bonus;
using Code.Logger;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public class BonusCollector : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_HeroData, c_CurrentGroundData, InventoryData>> _heroData = default;
        private readonly EcsFilterInject<Inc<BonusData>> _bonusData = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var heroEntity in _heroData.Value)
            {
                ref var heroData = ref _heroData.Pools.Inc1.Get(heroEntity);
                ref var currentGround = ref _heroData.Pools.Inc2.Get(heroEntity);
                ref var inventoryData = ref _heroData.Pools.Inc3.Get(heroEntity);
                if(currentGround.IsBaseGround) return;
                FindBonus(ref heroData, ref inventoryData);
            }
        }

        private void FindBonus(ref c_HeroData heroData, ref InventoryData inventoryData)
        {
            foreach (var entity in _bonusData.Value)
            {
                ref var bonusData = ref _bonusData.Pools.Inc1.Get(entity);
                if(!bonusData.BonusGameObject.activeSelf || bonusData.IsPickUp) continue;

                var distance = Vector3.Distance(heroData.HeroGameObject.transform.position, bonusData.BonusGameObject.transform.position);
                if (distance < heroData.BonusSearchRadius)
                {
                    bonusData.BonusRigidbody.isKinematic = true;
                    bonusData.IsPickUp = true;
                    var bonusSettings = bonusData.BonusGameObject.GetComponent<BonusSettings>().BonusEntity;
                    AddBonusInInventory(ref inventoryData, ref bonusSettings);
                }
            }
        }

        private void AddBonusInInventory(ref InventoryData inventoryData, ref int bonusEntities)
        {
            if (inventoryData.BonusEntities.Count < inventoryData.MaxStackSize)
            {
                inventoryData.BonusEntities.Add(bonusEntities);
                ref var bonusData = ref _bonusData.Pools.Inc1.Get(bonusEntities); 
                bonusData.BonusGameObject.transform.parent = inventoryData.InventoryObject;
                bonusData.BonusGameObject.transform.localRotation = Quaternion.identity;
                var position = inventoryData.CurrentPosition.position + inventoryData.CurrentPosition.up * inventoryData.CurrentOffsetPosition;
                bonusData.BonusGameObject.transform.position = position;
                inventoryData.CurrentOffsetPosition += inventoryData.DefaultOffsetPosition;
            }
        }
    }
}