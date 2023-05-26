using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Score
{
    public class ScoreInit : IEcsInitSystem
    {
        private readonly EcsPoolInject<ScoreData> _scoreData = default;
        private readonly EcsCustomInject<ScoreSettings> _scoreSettings = default;
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var scoreData = ref _scoreData.Value.Add(entity);

            scoreData.RegularBonusTextScore = _scoreSettings.Value.RegularBonusTextScore;
            scoreData.MegaBonusTextScore = _scoreSettings.Value.MegaBonusTextScore;
        }
    }
}