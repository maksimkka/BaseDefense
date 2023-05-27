using UnityEngine;

namespace Code.Spawner
{
    [DisallowMultipleComponent]
    public class EnemySpawnerSettings : MonoBehaviour
    {
        [field: SerializeField] public GameObject EnemyPrefab { get; private set; }
        [field: SerializeField] public MeshRenderer PlaneSpawnObject { get; private set; }
        [field: SerializeField] public float SpawnDelay { get; private set; }
        [field: SerializeField] public int PoolSize { get; private set; }
        [field: SerializeField] public int MaxSpawn { get; private set; }
    }
}