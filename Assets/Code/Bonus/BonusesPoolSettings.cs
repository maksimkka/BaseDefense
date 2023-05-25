using UnityEngine;

namespace Code.Bonus
{
    [DisallowMultipleComponent]
    public class BonusesPoolSettings : MonoBehaviour
    {
        [field: SerializeField] public GameObject RegularBonusPrefab{ get; private set; }
        [field: SerializeField] public GameObject MegaBonusPrefab{ get; private set; }
        [field: SerializeField] public int StartSizeRegularBonusPool{ get; private set; }
        [field: SerializeField] public int StartSizeMegaBonusPool{ get; private set; }
        [field: SerializeField] public float SpawnProbabilityRegularBonus{ get; private set; }
    }
}