using Code.Logger;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public class s_HeroMove : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_Hero>> _heroFilter;
        private readonly EcsCustomInject<FloatingJoystick> _joystick;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _heroFilter.Value)
            {
                ref var hero = ref _heroFilter.Pools.Inc1.Get(entity);
                Move(ref hero);
            }
        }

        private void Move(ref c_Hero hero)
        {
            hero.heroRigidBody.velocity = new Vector3(_joystick.Value.Horizontal * hero.Speed, hero.heroRigidBody.velocity.y ,_joystick.Value.Vertical * hero.Speed);
            if (_joystick.Value.Horizontal != 0 || _joystick.Value.Vertical != 0)
            {
                hero.HeroGameObject.transform.rotation = Quaternion.LookRotation(hero.heroRigidBody.velocity);
            }
        }
    }
}