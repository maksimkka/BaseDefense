using System.Threading.Tasks;
using Code.EndGame;
using Code.Enemy;
using Code.Hero;
using Code.Hero.Inventory;
using Code.Weapon;
using DG.Tweening;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.UI.Restart
{
    public class RestartGame : IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<PauseData>> _pauseFilter = default;
        private readonly EcsFilterInject<Inc<HeroData>> _heroFilter = default;
        private readonly EcsFilterInject<Inc<EnemyData>> _enemyFilter = default;
        private readonly EcsFilterInject<Inc<WeaponData>> _weaponFilter = default;
        private readonly EcsFilterInject<Inc<InventoryData>> _inventoryDataFilter = default;

        private readonly EcsPoolInject<ClearInventoryRequest> _clearInventoryRequest = default;
        private readonly EcsPoolInject<RestartButtonData> _restartButtonData = default;
        private readonly EcsCustomInject<RestartScreenView> _restartButtonView = default;
        private readonly EcsPoolInject<EndGameMarker> _endGame = default;
        private readonly EcsPoolInject<CanShootMarker> m_CanShoot = default;

        private bool _isRestart;


        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var restartButtonData = ref _restartButtonData.Value.Add(entity);

            restartButtonData.RestartButton = _restartButtonView.Value.RestartButton;
            restartButtonData.RestartButton.onClick.AddListener(NotifyTheDeathOfPlayer);
        }

        private async void NotifyTheDeathOfPlayer()
        {
            ToggleScreens(false);
            NotifyEnemies();
            NotifyWeapon();
            NotifyClearInventory();
            await NotifyHero();
            ToggleScreens(true);
            NotifyHero();
            NotifyEnemies();
            NotifyWeapon();
        }

        private Task NotifyHero()
        {
            Task move = null;
            foreach (var heroEntity in _heroFilter.Value)
            {
                ref var hero = ref _heroFilter.Pools.Inc1.Get(heroEntity);
                if (!_endGame.Value.Has(heroEntity))
                {
                    _endGame.Value.Add(heroEntity);
                    hero.HeroGameObject.SetActive(false);
                    move =  hero.HeroGameObject.transform.DOMove(hero.StartPosition.position, 1f)
                        .SetEase(Ease.Linear)
                        .AsyncWaitForCompletion();
                }

                else
                {
                    hero.CurrentHP = hero.DefaultHP;
                    hero.Slider.value = hero.DefaultHP;
                    hero.HeroGameObject.SetActive(true);
                    _endGame.Value.Del(heroEntity);
                }
            }

            return move ??= Task.CompletedTask;
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
                    enemy.States = EnemyStates.Idle;
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
                    m_CanShoot.Value.Del(weaponEntity);
                    _endGame.Value.Add(weaponEntity);
                }

                else
                {
                    _endGame.Value.Del(weaponEntity);
                }
            }
        }

        private void ToggleScreens(bool isShow)
        {
            foreach (var entity in _pauseFilter.Value)
            {
                ref var pauseData = ref _pauseFilter.Pools.Inc1.Get(entity);
                if (isShow)
                {
                    pauseData.GameScreen.gameObject.SetActive(true);
                }

                else
                {
                    pauseData.GameScreen.gameObject.SetActive(false);
                    pauseData.RestartScreen.gameObject.SetActive(false);
                }
            }
        }

        private void NotifyClearInventory()
        {
            foreach (var entity in _inventoryDataFilter.Value)
            {
                if (!_clearInventoryRequest.Value.Has(entity))
                {
                    ref var clearInventory = ref _clearInventoryRequest.Value.Add(entity);
                    clearInventory.IsRestart = true;
                }
            }
        }
    }
}