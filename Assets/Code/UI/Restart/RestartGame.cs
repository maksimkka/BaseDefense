using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.UI.Restart
{
    public class RestartGame : IEcsRunSystem
    {
        private EcsFilterInject<Inc<RestartButtonData>> _restartButtonFilter = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _restartButtonFilter.Value)
            {
                ref var restartButton = ref _restartButtonFilter.Pools.Inc1.Get(entity);
                //restartButton.RestartButton.onClick
            }
        }
    }
}