using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.HealthBar
{
    public sealed class i_HealthBar : IEcsInitSystem
    {
        private readonly EcsCustomInject<HealthBarView> _healthBar = default;
        private readonly EcsPoolInject<c_HealthBar> c_HealthBar;
        public void Init(IEcsSystems systems)
        {
            var entity = systems.GetWorld().NewEntity();
            ref var healthBar = ref c_HealthBar.Value.Add(entity);
            var count = _healthBar.Value.transform.childCount;
            healthBar.Health = new Queue<(RectTransform RectTranform, Slider Slider)>(count); 

            for (int i = 0; i < count; i++)
            {
                var rect = _healthBar.Value.transform.GetChild(i).GetComponent<RectTransform>();
                rect.gameObject.SetActive(false);
                var currentSlider = rect.gameObject.GetComponentInChildren<Slider>(true);
                healthBar.Health.Enqueue((rect, currentSlider));
            }
        }
    }
}