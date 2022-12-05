using KefirTest.ResourceManager.Interface;
using KefirTest.System.Interface;
using UI.Interface;
using UnityEngine;

namespace KefirTest.UI.Main
{
    public class MainWindowFactory : IWindowFactory<MainWindowController>
    {
        private readonly Canvas _canvas;
        private readonly IWindowResourceManager _windowResourceManager;
        private readonly IPlayerMovementGetter _playerMovementGetter;
        private readonly IScanHitWeaponStateGetter _weaponStateGetter;

        public MainWindowFactory(Canvas canvas,
            IWindowResourceManager windowResourceManager,
            IPlayerMovementGetter playerMovementGetter,
            IScanHitWeaponStateGetter weaponStateGetter)
        {
            _canvas = canvas;
            _windowResourceManager = windowResourceManager;
            _playerMovementGetter = playerMovementGetter;
            _weaponStateGetter = weaponStateGetter;
        }
        
        public MainWindowController CreateWindow()
        {
            MainWindowView view = _windowResourceManager.GetWindowView<MainWindowView>(_canvas.transform);
            view.transform.SetParent(_canvas.transform);
            MainWindowPresenter presenter = new MainWindowPresenter(view, _playerMovementGetter, _weaponStateGetter);
            MainWindowController mainWindowController = new MainWindowController(presenter);

            return mainWindowController;
        }
    }
}