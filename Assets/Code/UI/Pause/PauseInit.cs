using Code.Logger;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.UI
{
    public class PauseInit : IEcsInitSystem
    {
        private readonly EcsPoolInject<PauseData> _pauseData;
        private readonly EcsCustomInject<HUDSettings> _menu;
        private bool _isPause;
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var pauseData = ref _pauseData.Value.Add(entity);
            pauseData.OpenMenuButton = _menu.Value.OpenMenuButton;
            pauseData.RestartScreen = _menu.Value.RestartScreen;
            pauseData.GameScreen = _menu.Value.GameScreen;
            var data = pauseData;
            pauseData.OpenMenuButton.onClick.AddListener(() => Pause(data.RestartScreen));
        }

        private void Pause(GameObject RestartScreen)
        {
            _isPause = !_isPause;

            if (_isPause)
            {
                $"qweqweqweqweqwe".Colored(Color.cyan).Log();
                RestartScreen.SetActive(_isPause);
            }
            
            else
            {
                RestartScreen.SetActive(_isPause);
            }
        }
    }
}