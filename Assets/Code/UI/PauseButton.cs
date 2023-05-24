using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class PauseButton : MonoBehaviour
    {
        private Button _button;
        private bool _isPause;
        public GameObject RestartPanel;

        private void Start()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(Pause);
        }

        private void Pause()
        {
            _isPause = !_isPause;

            if (_isPause)
            {
                Time.timeScale = 0;
                RestartPanel.SetActive(_isPause);
            }
            else
            {
                Time.timeScale = 1;
                RestartPanel.SetActive(_isPause);
            }
        }

        public void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}