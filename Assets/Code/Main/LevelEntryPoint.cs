using System;
using System.Collections.Generic;
using System.Threading;
using Code.Bullet;
using Code.Enemy;
using Code.Ground;
using Code.Hero;
using Code.Spawner;
using Code.Weapon;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Main
{
    [DisallowMultipleComponent]
    public sealed class LevelEntryPoint : MonoBehaviour
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
            var WeaponSettings = FindObjectOfType<WeaponSettings>(true);
            var enemySettings = FindObjectsOfType<EnemySettings>(true);
            var groundSettings = FindObjectsOfType<GroundSettings>(true);

            foreach (var system in _systems)
            {
                system.Value
                    .Inject(heroSettings, joystick, enemySettings, groundSettings, spawnerSettings, WeaponSettings);
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
            _systems[SystemType.Update].Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
        }

        private void AddGameSystems()
        {
            _systems[SystemType.Init]
                .Add(new i_EnemySpawner())
                .Add(new i_HeroWeapon())
                .Add(new i_Bullet())
                .Add(new i_Hero())
                .Add(new i_Enemy())
                .Add(new i_Ground());

            _systems[SystemType.Update]
                .Add(new s_GroundChecker())
                .Add(new s_EnemiesSpawner())
                .Add(new s_HeroShooting())
                .Add(new s_BulletLifeCycle());

            _systems[SystemType.FixedUpdate]
                .Add(new s_HeroMove())
                .Add(new s_EnemyMove());
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