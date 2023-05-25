using UnityEngine;

namespace Code.Bonus
{
    [DisallowMultipleComponent]
    public class BonusSettings : MonoBehaviour
    {
        [field: SerializeField] public BonusType BonusType { get; private set; }
        [field: SerializeField] public int BonusValue { get; private set; }
        public int BonusEntity { get; set; }
    }
}