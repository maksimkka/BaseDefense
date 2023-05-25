using UnityEngine;

namespace Code.UI.Restart
{
    [DisallowMultipleComponent]
    public class RestartScreenView : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }
    }
}