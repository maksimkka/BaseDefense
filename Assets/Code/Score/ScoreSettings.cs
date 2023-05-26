using TMPro;
using UnityEngine;

namespace Code.Score
{
    [DisallowMultipleComponent]
    public class ScoreSettings : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI MegaBonusTextScore { get; private set; }
        [field: SerializeField] public TextMeshProUGUI RegularBonusTextScore { get; private set; } 
    }
}