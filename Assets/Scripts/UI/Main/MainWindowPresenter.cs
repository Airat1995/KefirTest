using KefirTest.System.Interface;
using KefirTest.UI.Interface;
using KefirTest.UI.Interface.Main;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace KefirTest.UI.Main
{
    public class MainWindowPresenter : IMainWindowPresenter, IWindowEventReceiver
    {
        private readonly IMainWindowView _view;
        private readonly IPlayerMovementGetter _playerMovementGetter;
        private readonly IScanHitWeaponStateGetter _weaponStateGetter;

        public MainWindowPresenter(IMainWindowView view, IPlayerMovementGetter playerMovementGetter, IScanHitWeaponStateGetter weaponStateGetter)
        {
            _view = view;
            _playerMovementGetter = playerMovementGetter;
            _weaponStateGetter = weaponStateGetter;
        }

        public void Open()
        {
            _view.Open();
        }

        public void Close()
        {
            _view.Close();
        }

        public void Update()
        {
            Vector2 playerPosition = _playerMovementGetter.GetPosition();
            _view.ShowPosition(playerPosition.x, playerPosition.y);
            _view.ShowRotation(_playerMovementGetter.GetRotation());
            _view.ShowSpeed(_playerMovementGetter.GetSpeed());
            _view.ShowScanhitWeaponCooldown(_weaponStateGetter.GetCooldown());
            _view.ShowScanhitWeaponMagazine(_weaponStateGetter.GetMagazineSize());
        }
    }
}