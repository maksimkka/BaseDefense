using UnityEngine;

namespace Code.Spawner
{
    public class SpawnerSettings : MonoBehaviour
    {
        [field: SerializeField] public GameObject EnemyPrefab { get; private set; }
    }
}