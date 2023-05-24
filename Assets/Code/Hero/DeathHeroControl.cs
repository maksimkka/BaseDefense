using Code.EndGame;
using Code.Enemy;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.UnityEditor;

namespace Code.Hero
{
    public class DeathHeroControl : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HeroDiedMarker>> _heroDiedFilter = default;
        private readonly EcsFilterInject<Inc<c_HeroData>> _heroFilter = default;
        private readonly EcsFilterInject<Inc<c_Enemy>> _enemyFilter = default;
        private readonly EcsFilterInject<Inc<c_WeaponData>> _weaponFilter = default;

        private readonly EcsPoolInject<EndGameMarker> _endGame = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _heroDiedFilter.Value)
            {
                var endGameEntity = systems.GetWorld().NewEntity();
                NotifyTheDeathOfPlayer(endGameEntity);
            }
        }

        private void NotifyTheDeathOfPlayer(int entity)
        {
            NotifyHero(entity);
            NotifyEnemies(entity);
            NotifyWeapon(entity);
        }

        private void NotifyHero(int entity)
        {
            foreach (var heroEntity in _heroFilter.Value)
            {
                if(!_heroFilter.Pools.Inc1.Has(entity))
                {
                    _heroFilter.Pools.Inc1.Add(entity);
                }
                else
                {
                    _heroFilter.Pools.Inc1.Del(entity);
                }
            }
        }

        private void NotifyEnemies(int entity)
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                if (!_enemyFilter.Pools.Inc1.Has(entity))
                {
                    _enemyFilter.Pools.Inc1.Add(entity);
                }

                else
                {
                    _enemyFilter.Pools.Inc1.Del(entity);
                }
            }
        }

        private void NotifyWeapon(int entity)
        {
            foreach (var weaponEntity in _weaponFilter.Value)
            {
                if (!_weaponFilter.Pools.Inc1.Has(entity))
                {
                    _weaponFilter.Pools.Inc1.Add(entity);
                }

                else
                {
                    _weaponFilter.Pools.Inc1.Del(entity);
                }
                
            }
        }
    }
}