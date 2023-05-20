using System;
using Code.Logger;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Main
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _endGameWinText;
        [SerializeField] private TextMeshProUGUI _endGameLoseText;

        private bool _isWin;
        public void ChangeState(bool isState)
        {
            gameObject.SetActive(true);

            _isWin = isState;
            if (_isWin)
            {
                _endGameWinText.gameObject.SetActive(true);
            }
            else
            {
                _endGameLoseText.gameObject.SetActive(true);
            }
        }

        private void Start()
        {
            //_button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        }

        public void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}