using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Hero
{
    public class InventoryInit : IEcsInitSystem
    {
        private readonly EcsPoolInject<InventoryData> _inventoryData = default;
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var inventoryData = ref _inventoryData.Value.Add(entity);
        }
    }
}