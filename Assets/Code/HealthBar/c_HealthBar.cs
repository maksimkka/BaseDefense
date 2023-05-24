using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Game.HealthBar
{
    public struct c_HealthBar
    {
        public Queue<(RectTransform RectTranform, Slider Slider)> Health;
    }
}