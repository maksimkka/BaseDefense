using System;
using System.Collections.Generic;
using System.Threading;
using Code.Bonus;
using Code.Bullet;
using Code.Enemy;
using Code.Game.HealthBar;
using Code.Ground;
using Code.Hero;
using Code.Hero.Inventory;
using Code.Score;
using Code.Spawner;
using Code.UI;
using Code.UI.Restart;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Main
{
    [DisallowMultipleComponent]
    public class LevelEntryPoint : MonoBehaviour
    {
        private readonly Dictionary<SystemType, EcsSystems> _systems = new();
        private readonly CancellationTokenSource _tokenSources = new();
        
        private EcsWorld _world;
        private EcsSystems _updateSystem;

        private void Start()
        {
            InitECS();
        }

        private void DistributeDataBetweenGameModes()
        {
            AddGameSystems();
            InjectGameObjects();
        }

        private void InitECS()
        {
            _world = new EcsWorld();
            var systemTypes = Enum.GetValues(typeof(SystemType));
            foreach (var item in systemTypes)
            {
                _systems.Add((SystemType)item, new EcsSystems(_world));
            }

#if UNITY_EDITOR
            AddDebugSystems();
#endif

            DistributeDataBetweenGameModes();

            foreach (var system in _systems)
            {
                system.Value.Init();
            }
        }

        private void InjectGameObjects()
        {
            var heroSettings = FindObjectOfType<HeroSettings>(true);
            var joystick = FindObjectOfType<FloatingJoystick>(true);
            var spawnerSettings = FindObjectOfType<EnemySpawnerSettings>(true);
            var bonusesPoolSettings = FindObjectOfType<BonusesPoolSettings>(true);
            var weaponSettings = FindObjectOfType<WeaponSettings>(true);
            var healthBarView = FindObjectOfType<HealthBarView>(true);
            var scoreSettings = FindObjectOfType<ScoreSettings>(true);
            var inventorySettings = FindObjectOfType<InventorySettings>(true);
            var restartButtonView = FindObjectOfType<RestartScreenView>(true);
            var menuSettings = FindObjectOfType<HUDSettings>(true);
            var enemySettings = FindObjectsOfType<EnemySettings>(true);
            var groundSettings = FindObjectsOfType<GroundMarker>(true);

            foreach (var system in _systems)
            {
                system.Value.Inject(heroSettings, joystick, enemySettings, groundSettings,
                        spawnerSettings, weaponSettings, healthBarView, restartButtonView,
                        menuSettings, bonusesPoolSettings, inventorySettings, scoreSettings);
            }
        }

        private void Update()
        {
            _systems[SystemType.Update].Run();
        }

        private void FixedUpdate()
        {
            _systems[SystemType.FixedUpdate].Run();
        }

        private void AddDebugSystems()
        {
#if UNITY_EDITOR
            _systems[SystemType.Update].Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
        }

        private void AddGameSystems()
        {
            _systems[SystemType.Init]
                .Add(new RestartGame())
                .Add(new ScoreInit())
                .Add(new PauseInit())
                .Add(new HealthBarInit())
                .Add(new BonusSpawnerInit())
                .Add(new EnemySpawnerInit())
                .Add(new WeaponInit())
                .Add(new BulletInit())
                .Add(new HeroInit(_tokenSources))
                .Add(new EnemyInit(_tokenSources))
                .Add(new GroundInit());

            _systems[SystemType.Update]
                .Add(new GroundChecker())
                .Add(new ChangerStateEnemies())
                .Add(new EnemiesSpawner())
                .Add(new EnemyMove())
                .Add(new EnemyAttack())
                .Add(new WeaponShooting())
                .Add(new BulletLifeCycle())
                .Add(new BulletCollision())
                .Add(new ReturnerBulletToPool())
                .Add(new EnemyHitHandling())
                .Add(new ManagingBonusPool())
                .Add(new HeroDamageHandler())
                .Add(new BonusCollector())
                .Add(new InventoryCleaning())
                .Add(new ChangerScore())
                .Add(new RegenerateHP());

            _systems[SystemType.FixedUpdate]
                .Add(new HeroMove())
                .Add(new HealthBarMove());

        }

        private void OnDestroy()
        {
            _world?.Destroy();
            foreach (var system in _systems)
            {
                system.Value.Destroy();
            }

            _tokenSources.Cancel();
            _tokenSources.Dispose();
        }
    }
}