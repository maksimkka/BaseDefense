using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    [DisallowMultipleComponent]
    public class HUDSettings : MonoBehaviour
    {
        [field: SerializeField] public Button OpenMenuButton { get; private set; }
        [field: SerializeField] public GameObject RestartScreen { get; private set; }
        [field: SerializeField] public GameObject GameScreen { get; private set; }

        public void OnDestroy()
        {
            OpenMenuButton.onClick.RemoveAllListeners();
        }
    }
}