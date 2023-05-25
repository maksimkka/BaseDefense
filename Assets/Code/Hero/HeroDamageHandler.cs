using Code.Enemy;
using Code.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Hero
{
    public class HeroDamageHandler : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_HeroData, SubmitDamageRequest>> _heroDamagedFilter = default;
        private readonly EcsFilterInject<Inc<PauseData>> _pauseFilter = default;
        private readonly EcsFilterInject<Inc<c_Enemy>> _enemyFilter = default;
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
                //ChangeStateEnemies();
                SwitchScreens();
                heroData.CurrentHP = 0;
                heroData.HeroGameObject.gameObject.SetActive(false);
                _heroDamagedFilter.Pools.Inc2.Del(entity);
            }
        }

        private void SwitchScreens()
        {
            foreach (var entity in _pauseFilter.Value)
            {
                ref var pause = ref _pauseFilter.Pools.Inc1.Get(entity);
                
                pause.GameScreen.gameObject.SetActive(false);
                pause.RestartScreen.gameObject.SetActive(true);
            }
        }

        private void ChangeStateEnemies()
        {
            foreach (var entity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(entity);
                enemy.States = EnemyStates.Idle;
            }
        }
    }
}