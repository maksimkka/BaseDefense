using Code.Logger;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public sealed class s_HeroMove : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<c_HeroData>> _heroFilter = default;
        private readonly EcsCustomInject<FloatingJoystick> _joystick = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _heroFilter.Value)
            {
                ref var hero = ref _heroFilter.Pools.Inc1.Get(entity);
                Move(ref hero);
            }
        }

        private void Move(ref c_HeroData heroData)
        {
            heroData.heroRigidBody.velocity = new Vector3(_joystick.Value.Horizontal * heroData.Speed, heroData.heroRigidBody.velocity.y ,_joystick.Value.Vertical * heroData.Speed);
            if (_joystick.Value.Horizontal != 0 || _joystick.Value.Vertical != 0)
            {
                heroData.HeroGameObject.transform.rotation = Quaternion.LookRotation(new Vector3(heroData.heroRigidBody.velocity.x, 0, heroData.heroRigidBody.velocity.z));
            }
        }
    }
}