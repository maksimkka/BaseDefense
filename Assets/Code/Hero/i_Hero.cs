using System.Collections.Generic;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Hero
{
    public sealed class i_Hero : IEcsInitSystem
    {
        private readonly EcsPoolInject<c_HeroData> c_Hero = default;
        private readonly EcsPoolInject<c_CurrentGroundData> c_CurrentGroundData = default;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> c_UnityPhysicsCollisionDataComponent = default;
        private readonly EcsCustomInject<HeroSettings> _heroSettings = default;
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var currentGroundData = ref c_CurrentGroundData.Value.Add(entity);
            currentGroundData.IsBaseGround = true;
            currentGroundData.Detector = _heroSettings.Value.GetComponentInChildren<UnityPhysicsCollisionDetector>();
            currentGroundData.Detector.Init(entity, systems.GetWorld());
            
            ref var c_collisionData = ref c_UnityPhysicsCollisionDataComponent.Value.Add(entity);
            c_collisionData.CollisionsEnter = new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();

            ref var hero = ref c_Hero.Value.Add(entity);
            hero.HeroGameObject = _heroSettings.Value.gameObject;
            hero.heroRigidBody = _heroSettings.Value.GetComponent<Rigidbody>();
            hero.Speed = _heroSettings.Value.Speed;
            hero.HP = _heroSettings.Value.HP;
        }
    }
}