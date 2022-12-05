using UI.Interface.GameOver;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameOver
{
    public class GameOverWindowView : MonoBehaviour, IGameOverView
    {
        [SerializeField]
        private Text _showScoreText;

        [SerializeField]
        private Button _resetButton;
        
        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void ShowScore(float score)
        {
            _showScoreText.text = $"Score: {score}";
        }

        public void SubscribeEventReceiver(IGameOverEventReceiver eventReceiver)
        {
            _resetButton.onClick.AddListener(eventReceiver.Reset);
        }
    }
}