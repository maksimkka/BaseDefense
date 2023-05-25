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
                if(!bonusData.BonusGameObject.activeSelf && bonusData.IsPickUp) continue;

                var distance = Vector3.Distance(heroData.HeroGameObject.transform.position, bonusData.BonusGameObject.transform.position);

                if (distance < heroData.BonusSearchRadius)
                {
                    bonusData.BonusRigidbody.isKinematic = true;
                    bonusData.IsPickUp = true;
                    AddBonusInInventory(ref inventoryData, ref bonusData.BonusGameObject);
                    //inventoryData.StackInventory.Add();
                    //bonusData.BonusGameObject.SetActive(false);
                }
            }
        }

        private void AddBonusInInventory(ref InventoryData inventoryData, ref GameObject bonus)
        {
            if (inventoryData.StackInventory.Count < inventoryData.MaxStackSize)
            {
                inventoryData.StackInventory.Add(bonus);
                UpdateItemPositions(ref inventoryData);
                bonus.transform.parent = inventoryData.InventoryObject;
            }
        }
        
        private void UpdateItemPositions(ref InventoryData inventoryData)
        {
            float offset = 0.1f;
            for (int i = 0; i < inventoryData.StackInventory.Count; i++)
            {
                var position = inventoryData.CurrentPosition.position + inventoryData.CurrentPosition.up * offset * i;
                inventoryData.StackInventory[i].transform.position = position;
            }
        }
    }
}