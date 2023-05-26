using System.Collections.Generic;
using System.Threading;
using Code.Bonus;
using Code.Game.HealthBar;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Hero
{
    public sealed class i_Hero : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<c_HealthBar>> _healthBarFilter = default;
        private readonly EcsPoolInject<c_HeroData> c_Hero = default;
        private readonly EcsPoolInject<c_CurrentGroundData> c_CurrentGroundData = default;
        private readonly EcsPoolInject<c_HealthBarData> c_HealthBarData = default;
        private readonly EcsPoolInject<InventoryData> _inventoryData = default;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> c_UnityPhysicsCollisionDataComponent = default;
        private readonly EcsCustomInject<HeroSettings> _heroSettings = default;
        private readonly EcsCustomInject<InventorySettings> _inventorySettings = default;
        
        private readonly int _idleAnimation = Animator.StringToHash("DynIdle");
        private readonly int _runAnimation = Animator.StringToHash("Running");
        private readonly int _riffleWalkAnimation = Animator.StringToHash("RiffleWalk");
        private readonly int _riffleIdleAnimation = Animator.StringToHash("RiffleIdle");
        private readonly CancellationTokenSource _tokenSources;

        public i_Hero(CancellationTokenSource tokenSource)
        {
            _tokenSources = tokenSource;
        }
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var currentGroundData = ref c_CurrentGroundData.Value.Add(entity);
            currentGroundData.IsBaseGround = true;
            currentGroundData.Detector = _heroSettings.Value.GetComponentInChildren<UnityPhysicsCollisionDetector>();
            currentGroundData.Detector.Init(entity, systems.GetWorld());
            
            ref var c_collisionData = ref c_UnityPhysicsCollisionDataComponent.Value.Add(entity);
            c_collisionData.CollisionsEnter = new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();

            var heroAnimator = _heroSettings.Value.GetComponentInChildren<Animator>();
            InitInventory(entity);
            ref var hero = ref c_Hero.Value.Add(entity);
            hero.Animator = new AnimationSwitcher(heroAnimator, _tokenSources);
            hero.HeroGameObject = _heroSettings.Value.gameObject;
            hero.HeroRigidBody = _heroSettings.Value.GetComponent<Rigidbody>();
            hero.HealthPosition = _heroSettings.Value.HealthPosition;
            hero.Speed = _heroSettings.Value.Speed;
            hero.CurrentHP = _heroSettings.Value.HP;
            hero.DefaultHP = _heroSettings.Value.HP;
            hero.RegenDelay = _heroSettings.Value.RegenDelay;
            hero.RegenHPTimer = _heroSettings.Value.RegenDelay;
            hero.RegenRate = _heroSettings.Value.RegenRate;
            hero.Distance = _heroSettings.Value.Distance;
            hero.IdleAnimationHash = _idleAnimation;
            hero.RunAnimationHash = _runAnimation;
            hero.RiffleWalkAnimationHash = _riffleWalkAnimation;
            hero.RiffleIdleAnimation = _riffleIdleAnimation;
            hero.StartPosition = _heroSettings.Value.HeroStartPosition;
            hero.BonusSearchRadius = _heroSettings.Value.BonusSearchRadius;

            InitHealthBar(ref hero);
        }

        private void InitHealthBar(ref c_HeroData heroData)
        {
            foreach (var entity in _healthBarFilter.Value)
            {
                ref var healthBar = ref _healthBarFilter.Pools.Inc1.Get(entity);
                
                (RectTransform RectTransform, Slider Slider) valueTuple = healthBar.Health.Peek();
                healthBar.Health.Dequeue();
                
                heroData.Slider = valueTuple.Slider;
                heroData.Slider.maxValue = heroData.CurrentHP;
                heroData.Slider.value = heroData.CurrentHP;
                ref var c_healthBarData = ref c_HealthBarData.Value.Add(entity);
            
                c_healthBarData.RectTransform = valueTuple.RectTransform;
                c_healthBarData.GameObject = heroData.HealthPosition;
                
                valueTuple.RectTransform.gameObject.SetActive(true);
            }
            
        }

        private void InitInventory(int entity)
        {
            ref var inventoryData = ref _inventoryData.Value.Add(entity);
            inventoryData.InventoryObject = _inventorySettings.Value.gameObject.transform;
            inventoryData.CurrentPosition = inventoryData.InventoryObject;
            inventoryData.MaxStackSize = _inventorySettings.Value.MaxStackSize;
            inventoryData.DefaultOffsetPosition = _inventorySettings.Value.OffsetPosition;
            inventoryData.BonusEntities = new List<int>();
        }
    }
}