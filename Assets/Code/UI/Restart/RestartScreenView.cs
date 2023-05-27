using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Restart
{
    [DisallowMultipleComponent]
    public class RestartScreenView : MonoBehaviour
    {
        [field: SerializeField] public Button RestartButton { get; private set; }
        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }

        private void OnDestroy()
        {
            RestartButton.onClick.RemoveAllListeners();
        }
    }
}