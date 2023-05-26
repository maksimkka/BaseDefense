using Code.Bonus;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Code.Score
{
    public class ChangerScore : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ScoreData, ScoreChangeRequest>> _scoreDataFilter = default;
        private readonly EcsPoolInject<BonusData> _bonusData = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _scoreDataFilter.Value)
            {
                ref var scoreData = ref _scoreDataFilter.Pools.Inc1.Get(entity);
                ref var scoreChangeRequest = ref _scoreDataFilter.Pools.Inc2.Get(entity);
                
                for (int i = 0; i < scoreChangeRequest.StackInventory.Count; i++)
                {
                    ref var bonusData = ref _bonusData.Value.Get(scoreChangeRequest.StackInventory[i]);

                    if (bonusData.BonusType == BonusType.RegularBonus)
                    {
                        scoreData.CurrentRegularBonusScore += bonusData.BonusValue;
                        scoreData.RegularBonusTextScore.text = scoreData.CurrentRegularBonusScore.ToString();
                    }
                    else
                    {
                        scoreData.CurrentMegaBonusScore += bonusData.BonusValue;
                        scoreData.MegaBonusTextScore.text = scoreData.CurrentMegaBonusScore.ToString();
                    }
                }
                
                scoreChangeRequest.StackInventory.Clear();
                _scoreDataFilter.Pools.Inc2.Del(entity);
            }
        }
    }
}