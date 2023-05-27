using Code.Pools;
using UnityEngine;

namespace Code.Bonus
{
    public struct BonusSpawnerData
    {
        public ObjectPool<Collider> RegularBonusPool;
        public ObjectPool<Collider> MegaBonusPool;
        public GameObject SpawnerParentObject;
        public GameObject RegularBonusPrefab;
        public GameObject MegaBonusPrefab;
        public int StartSizeRegularBonusPool;
        public int StartSizeMegaBonusPool;
        public float SpawnProbabilityRegularBonus;
    }
}