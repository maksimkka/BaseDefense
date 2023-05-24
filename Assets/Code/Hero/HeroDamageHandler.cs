using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Hero
{
    public class HeroDamageHandler : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_HeroData, SubmitDamageRequest>> _heroDamagedFilter = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _heroDamagedFilter.Value)
            {
                GiveDamage(entity);
            }
        }

        private void GiveDamage(int entity)
        {
            ref var submitDamage = ref _heroDamagedFilter.Pools.Inc2.Get(entity);
            ref var heroData = ref _heroDamagedFilter.Pools.Inc1.Get(entity);
            heroData.CurrentHP -= submitDamage.DamageDone;
            heroData.Slider.value -= submitDamage.DamageDone;
            submitDamage.QueueForDamage -= 1;

            if (submitDamage.QueueForDamage <= 0)
            {
                _heroDamagedFilter.Pools.Inc2.Del(entity);
            }

            if (heroData.CurrentHP <= 0)
            {
                heroData.HeroGameObject.gameObject.SetActive(false);
                _heroDamagedFilter.Pools.Inc1.Del(entity);
            }
        }
    }
}