using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Code.Game.HealthBar
{
    public class HealthBarMove : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<HealthBarDataForHero>> _healsBarFilter = default;
        private readonly EcsPoolInject<HealthBarDataForHero> c_HealthBarData = default;
        private readonly Camera _camera;

        public HealthBarMove()
        {
            _camera = Camera.main;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _healsBarFilter.Value)
            {
                ref var healthBarData = ref c_HealthBarData.Value.Get(entity);
                var position = healthBarData.GameObject.transform.position;
                var newPosition = _camera.WorldToScreenPoint(position);
                healthBarData.RectTransform.position = WithChangedAxes(newPosition, z: 0f);
            }
        }

        private Vector3 WithChangedAxes(Vector3 vector, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        }
    }
}