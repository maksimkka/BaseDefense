using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public class RegenerateHP : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HeroData, RegenerateHPMarker>> _heroFilter = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _heroFilter.Value)
            {
                ref var heroData = ref _heroFilter.Pools.Inc1.Get(entity);
                if (heroData.CurrentHP < heroData.DefaultHP)
                {
                    heroData.RegenHPTimer -= Time.deltaTime;

                    if (heroData.RegenHPTimer <= 0f)
                    {
                        heroData.CurrentHP += heroData.RegenRate;
                        heroData.Slider.value += heroData.RegenRate;
                        heroData.RegenHPTimer = heroData.RegenDelay;
                    }
                }
            }
        }
    }
}