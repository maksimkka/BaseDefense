using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.HealthBar
{
    public struct HealthBarData
    {
        public Queue<(RectTransform RectTranform, Slider Slider)> Health;
    }
}