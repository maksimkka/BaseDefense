using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public class i_Hero : IEcsInitSystem
    {
        private readonly EcsPoolInject<c_Hero> c_Hero;
        private readonly EcsCustomInject<HeroSettings> _heroSettings;
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var hero = ref c_Hero.Value.Add(entity);
            hero.HeroGameObject = _heroSettings.Value.gameObject;
            hero.heroRigidBody = _heroSettings.Value.GetComponent<Rigidbody>();
            hero.Speed = _heroSettings.Value.Speed;
        }
    }
}