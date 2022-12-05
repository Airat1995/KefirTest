using KefirTest.UI.Interface.Main;
using UnityEngine;
using UnityEngine.UI;

namespace KefirTest.UI.Main
{
    public class MainWindowView : MonoBehaviour, IMainWindowView
    {
        [SerializeField]
        private Text _positionText;

        [SerializeField]
        private Text _speedText;

        [SerializeField]
        private Text _rotationText;

        [SerializeField]
        private Text _magazineSizeText;

        [SerializeField]
        private Text _scanhitCooldownText;

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void ShowPosition(float x, float y)
        {
            _positionText.text = $"Position: {x} x {y}";
        }

        public void ShowSpeed(float speed)
        {
            _speedText.text = $"Speed: {speed}";
        }

        public void ShowRotation(float rotation)
        {
            _rotationText.text = $"Rotation: {rotation}";
        }

        public void ShowScanhitWeaponMagazine(uint magazine)
        {
            _magazineSizeText.text = $"Magazine size: {magazine}";
        }

        public void ShowScanhitWeaponCooldown(float cooldown)
        {
            _scanhitCooldownText.text = $"Scanhit weapon cooldown: {cooldown}";
        }
    }
}