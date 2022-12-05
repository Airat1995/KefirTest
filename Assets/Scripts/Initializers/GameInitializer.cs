using System;
using System.Collections.Generic;
using System.Linq;
using KefirTest.Data;
using KefirTest.Input;
using KefirTest.ResourceManager.Interface;
using KefirTest.System;
using KefirTest.System.Interface;
using KefirTest.UI.Main;
using KefirTest.View;
using UI.GameOver;
using UnityEngine;

namespace KefirTest.Initializers
{
    public class GameInitializer : MonoBehaviour, IRessetable, IPausable
    {
        [Header("Player")]
        [SerializeField]
        private string _playerViewName;

        private Player _player;

        [SerializeField]
        private Vector3 _playerStartPosition;

        [SerializeField]
        private Vector3 _playerStartRotation;

        [SerializeField]
        private PlayerStatSO _playerStat;

        [SerializeField]
        private string _projectileViewName;

        private Projectile _projectile;

        [SerializeField]
        private ProjectileWeaponStatSO _projectileStatSo;

        [SerializeField]
        private string _scanHitViewName;

        private ScanHit _scanHitView;

        [SerializeField]
        private ScanHitWeaponStatSO _scanHitWeaponStatSo;

        [Header("Enemy")]
        [SerializeField]
        private LayerMask _enemyLayers;

        [SerializeField]
        private List<EnemyDescriptor> _enemies;

        [Header("General")]
        [SerializeField]
        private InputReceiver _inputReceiver;

        [SerializeField]
        private Camera _playerCamera;

        [SerializeField]
        private Canvas _canvas;

        private PlayerMovementSystem _playerMovementSystem;
        private ObjectScreenTeleporter _objectScreenTeleporter;
        private ProjectileWeaponSystem _projectileSystem;
        private ScanHitWeaponSystem _scanHitWeaponSystem;
        private PlayerScoreSystem _playerScoreSystem;
        private EnemySystem _enemySystem;
        private PlayerDamageSystem _playerDamageSystem;
        private bool _updateStopped;

        private MainWindowController _mainWindow;
        private GameOverWindowFactory _gameOverWindowFactory;
        private GameOverWindowController _gameOverWindow;

        private void Start()
        {
            ResourceManager.ResourceManager resourceManager = new ResourceManager.ResourceManager();

            LoadEnemyResources(resourceManager);
            LoadPlayerResources(resourceManager);

            _playerMovementSystem = new PlayerMovementSystem(_player, _playerStat, _inputReceiver, _playerStartPosition,
                _playerStartRotation);

            Bounds cameraBounds = OrthographicBounds(_playerCamera);
            _objectScreenTeleporter = new ObjectScreenTeleporter(_player, cameraBounds);

            Dictionary<Type, EnemyStatSO> enemies = GetEnemiesDict();
            _playerScoreSystem = new PlayerScoreSystem(enemies);

            _enemySystem = new EnemySystem(_playerScoreSystem, _enemies, _player, cameraBounds);

            _projectileSystem =
                new ProjectileWeaponSystem(_inputReceiver, _enemySystem, _player, _projectile, _projectileStatSo);

            _playerDamageSystem = new PlayerDamageSystem(_player, this);

            _scanHitWeaponSystem = new ScanHitWeaponSystem(_inputReceiver, _enemySystem, _player, _scanHitWeaponStatSo,
                _enemyLayers, _scanHitView);

            MainWindowFactory mainWindowFactory = new MainWindowFactory(_canvas, resourceManager, _playerMovementSystem,
                _scanHitWeaponSystem);
            _mainWindow = mainWindowFactory.CreateWindow();
            _mainWindow.Open();

            _gameOverWindowFactory = new GameOverWindowFactory(_canvas, resourceManager, this, _playerScoreSystem);
        }

        private void LoadEnemyResources(IGameEntityResourceManager resourceManager)
        {
            foreach (EnemyDescriptor enemyDescriptor in _enemies)
            {
                enemyDescriptor.AssignedVisual = resourceManager.GetEntity<Enemy>(enemyDescriptor.AssignedVisualName);
            }
        }

        private void LoadPlayerResources(IGameEntityResourceManager resourceManager)
        {
            Player playerResource = resourceManager.GetEntity<Player>(_playerViewName);
            _player = Instantiate(playerResource);
            _projectile = resourceManager.GetEntity<Projectile>(_projectileViewName);
            _scanHitView = resourceManager.GetEntity<ScanHit>(_scanHitViewName);
        }

        private Bounds OrthographicBounds(Camera camera)
        {
            float screenAspect = Screen.width / (float) Screen.height;
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(camera.transform.position,
                new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
            return bounds;
        }

        private Dictionary<Type, EnemyStatSO> GetEnemiesDict()
        {
            return _enemies.ToDictionary(desc => desc.AssignedVisual.GetType(), desc => desc.EnemyData);
        }

        private void Update()
        {
            if (_updateStopped)
                return;
            _mainWindow.Update(Time.deltaTime);
            _objectScreenTeleporter.Update(Time.deltaTime);
            _projectileSystem.Update(Time.deltaTime);

            _scanHitWeaponSystem.Update(Time.deltaTime);
            _enemySystem.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (_updateStopped)
                return;
            _playerMovementSystem.FixedUpdate(Time.deltaTime);
            _enemySystem.FixedUpdate(Time.deltaTime);
        }

        public void Reset()
        {
            _gameOverWindow.Close();
            Pause(false);
            _enemySystem.Reset();
            _playerMovementSystem.Reset();
            _playerScoreSystem.Reset();
            _scanHitWeaponSystem.Reset();
            _projectileSystem.Reset();
        }

        public void Pause(bool pause)
        {
            if (pause)
            {
                _gameOverWindow ??= _gameOverWindowFactory.CreateWindow();
                _gameOverWindow.Open();
            }

            _updateStopped = pause;
        }
    }
}
