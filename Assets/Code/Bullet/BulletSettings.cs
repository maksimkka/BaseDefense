using Code.Logger;
using UnityEngine;

namespace Code.Bullet
{
    [DisallowMultipleComponent]
    public sealed class BulletSettings : MonoBehaviour
    {
        [field: SerializeField] public float DefaultLifeTime { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        public int Entity { get; private set; }
        
        public void SetEntity(int entity)
        {
            Entity = entity;
        }
    }
}