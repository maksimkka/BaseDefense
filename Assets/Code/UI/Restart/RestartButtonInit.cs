using System;
using Code.EndGame;
using Code.Enemy;
using Code.Hero;
using Code.Weapon;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Restart
{
    public class RestartButtonInit : IEcsInitSystem
    {
        private readonly EcsPoolInject<RestartButtonData> _restartButtonData = default;
        private readonly EcsCustomInject<RestartScreenView> _restartButtonView = default;

        private readonly EcsFilterInject<Inc<HeroDiedMarker>> _heroDiedFilter = default;
        private readonly EcsFilterInject<Inc<c_HeroData>> _heroFilter = default;
        private readonly EcsFilterInject<Inc<c_Enemy>> _enemyFilter = default;
        private readonly EcsFilterInject<Inc<c_WeaponData>> _weaponFilter = default;

        private readonly EcsPoolInject<EndGameMarker> _endGame = default;

        private IEcsSystems _systems;
        private bool _isRestart;

        public void Init(IEcsSystems systems)
        {
            _systems = systems;
            var entity = systems.GetWorld().NewEntity();
            ref var restartButtonData = ref _restartButtonData.Value.Add(entity);
            var button = _restartButtonView.Value.GetComponentInChildren<Button>();

            restartButtonData.RestartButton = button;
            restartButtonData.RestartButton.onClick.AddListener(() =>
                NotifyTheDeathOfPlayer(_restartButtonView.Value.gameObject));
        }

        private async void NotifyTheDeathOfPlayer(GameObject gameObject)
        {
            //foreach (var entity in _heroDiedFilter.Value)
            //{
            //var endGameEntity = _systems.GetWorld().NewEntity();
            // var endGame = _endGame.Value.Add(endGameEntity);
            //NotifyTheDeathOfPlayer(endGameEntity);

            //if (_isRestart)
            //{
            gameObject.SetActive(false);
            NotifyHero();
            NotifyEnemies();
            NotifyWeapon();
            Time.timeScale = 1;
            await UniTask.Delay(TimeSpan.FromSeconds(1.1f));
            //}
            // else
            // {
            NotifyHero();
            NotifyEnemies();
            NotifyWeapon();
            // }
            //}
        }

        private void NotifyHero()
        {
            foreach (var heroEntity in _heroFilter.Value)
            {
                //var endGame = ;
                ref var hero = ref _heroFilter.Pools.Inc1.Get(heroEntity);
                if (!_endGame.Value.Has(heroEntity))
                {
                    _endGame.Value.Add(heroEntity);
                    hero.HeroGameObject.transform.DOMove(hero.StartPosition.position, 1f);
                    //_heroFilter.Pools.Inc1.Add(entity);
                }
                else
                {
                    _endGame.Value.Del(heroEntity);
                }
            }
        }

        private void NotifyEnemies()
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                ref var enemy = ref _enemyFilter.Pools.Inc1.Get(enemyEntity);
                if (!_endGame.Value.Has(enemyEntity))
                {
                    _endGame.Value.Add(enemyEntity);
                    enemy.IsHeroOnBase = true;
                    // enemy.States = EnemyStates.Idle;
                    // enemy.TargetMove = null;
                    // enemy.AnimationSwitcher.PlayAnimation(enemy.IdleAnimationHash);
                    // //ChangeNavMeshState(ref enemy, true);
                    // enemy.IsReadyAttack = false;
                    // enemy.CurrentReloadTime = 0;
                }

                else
                {
                    _endGame.Value.Del(enemyEntity);
                }
            }
        }

        private void NotifyWeapon()
        {
            foreach (var weaponEntity in _weaponFilter.Value)
            {
                if (!_endGame.Value.Has(weaponEntity))
                {
                    _endGame.Value.Add(weaponEntity);
                }

                else
                {
                    _endGame.Value.Del(weaponEntity);
                }
            }
        }
    }
}