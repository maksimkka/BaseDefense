using System.Collections.Generic;
using System.Threading;
using Code.Animations;
using Code.Game.HealthBar;
using Code.Ground;
using Code.Hero.Inventory;
using Code.UnityPhysics;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Hero
{
    public class HeroInit : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<HealthBarData>> _healthBarFilter = default;
        private readonly EcsPoolInject<HeroData> _heroData = default;
        private readonly EcsPoolInject<CurrentGroundData> _currentGroundData = default;
        private readonly EcsPoolInject<HealthBarDataForHero> _healthBarData = default;
        private readonly EcsPoolInject<InventoryData> _inventoryData = default;
        private readonly EcsPoolInject<UnityPhysicsCollisionDataComponent> _unityPhysicsCollisionDataComponent = default;
        private readonly EcsCustomInject<HeroSettings> _heroSettings = default;
        private readonly EcsCustomInject<InventorySettings> _inventorySettings = default;
        
        private readonly int _idleAnimation = Animator.StringToHash("DynIdle");
        private readonly int _runAnimation = Animator.StringToHash("Running");
        private readonly int _riffleWalkAnimation = Animator.StringToHash("RiffleWalk");
        private readonly int _riffleIdleAnimation = Animator.StringToHash("RiffleIdle");
        private readonly CancellationTokenSource _tokenSources;

        public HeroInit(CancellationTokenSource tokenSource)
        {
            _tokenSources = tokenSource;
        }
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var currentGroundData = ref _currentGroundData.Value.Add(entity);
            currentGroundData.IsBaseGround = true;
            currentGroundData.Detector = _heroSettings.Value.GetComponentInChildren<UnityPhysicsCollisionDetector>();
            currentGroundData.Detector.Init(entity, systems.GetWorld());
            
            ref var c_collisionData = ref _unityPhysicsCollisionDataComponent.Value.Add(entity);
            c_collisionData.CollisionsEnter = new Queue<(int layer, UnityPhysicsCollisionDTO collisionDTO)>();

            var heroAnimator = _heroSettings.Value.GetComponentInChildren<Animator>();
            InitInventory(entity);
            ref var hero = ref _heroData.Value.Add(entity);
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
            hero.DetectionDistance = _heroSettings.Value.DetectionDistance;
            hero.IdleAnimationHash = _idleAnimation;
            hero.RunAnimationHash = _runAnimation;
            hero.RiffleWalkAnimationHash = _riffleWalkAnimation;
            hero.RiffleIdleAnimation = _riffleIdleAnimation;
            hero.StartPosition = _heroSettings.Value.HeroStartPosition;
            hero.BonusSearchRadius = _heroSettings.Value.BonusSearchRadius;

            InitHealthBar(ref hero);
        }

        private void InitHealthBar(ref HeroData heroData)
        {
            foreach (var entity in _healthBarFilter.Value)
            {
                ref var healthBar = ref _healthBarFilter.Pools.Inc1.Get(entity);
                
                (RectTransform RectTransform, Slider Slider) valueTuple = healthBar.Health.Peek();
                healthBar.Health.Dequeue();
                
                heroData.Slider = valueTuple.Slider;
                heroData.Slider.maxValue = heroData.CurrentHP;
                heroData.Slider.value = heroData.CurrentHP;
                ref var c_healthBarData = ref _healthBarData.Value.Add(entity);
            
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