using System;
using KefirTest.Data;
using KefirTest.Input;
using KefirTest.System.Interface;
using KefirTest.View;
using UnityEngine;

namespace KefirTest.System
{
    public class ScanHitWeaponSystem : IResettableSystem, IUpdatableSystem, IScanHitWeaponStateGetter
    {
        private const int RAYCAST_HIT_MAX = 20;
        
        private readonly IInputRetranslator _inputRetranslator;
        private readonly IEnemyDestroyNotifier _enemyDestroyNotifier;
        private readonly IMovableElement _player;
        private readonly ScanHitWeaponStatSO _weaponStatSo;
        private RaycastHit2D[] _raycastHits = new RaycastHit2D[RAYCAST_HIT_MAX];
        private readonly int _enemyLayerMask;
        private readonly ObjectPool<GameEntity> _scanhitPool;
        private GameEntity _scanhitView;

        #region resettable data
        private float _timeBeforeNextCharge;
        private uint _chargesLeft;
        private float _scanhitShowTime;
        #endregion

        public ScanHitWeaponSystem(IInputRetranslator inputRetranslator,
            IEnemyDestroyNotifier enemyDestroyNotifier,
            IMovableElement player,
            ScanHitWeaponStatSO weaponStatSo,
            LayerMask enemyLayers,
            GameEntity scanHitView)
        {
            _inputRetranslator = inputRetranslator;
            _enemyDestroyNotifier = enemyDestroyNotifier;
            _player = player;
            _weaponStatSo = weaponStatSo;
            _enemyLayerMask = enemyLayers;
            _chargesLeft = 0;
            _scanhitPool = new ObjectPool<GameEntity>(scanHitView, 1, entity =>
            {

            });
            _inputRetranslator.SubscribeOnAltFire(Fire);
        }

        private void Fire()
        {
            if(_chargesLeft <= 0)
                return;
            int size = Physics2D.RaycastNonAlloc( _player.GetPosition(), _player.GetForwardVector(), _raycastHits, float.PositiveInfinity, _enemyLayerMask);
            for (int raycastIndex = 0; raycastIndex < size; ++raycastIndex)
            {
                Enemy enemy = _raycastHits[raycastIndex].collider.gameObject.GetComponent<Enemy>();
                if(enemy == null)
                    continue;
                _enemyDestroyNotifier.EnemyDestroy(enemy, true);
            }

            _scanhitShowTime = _weaponStatSo.VisibilityTime;
            --_chargesLeft;
            if(_scanhitView != null)
                _scanhitPool.ReturnToPool(_scanhitView);
            _scanhitView = _scanhitPool.GetEntity();
            _scanhitView.Enable(true);
            _scanhitView.SetPosition(_player.GetPosition() + new Vector3(0, 0, 1));
            _scanhitView.SetRotation(_player.GetRotation());
        }
        
        public void Reset()
        {
            _chargesLeft = 0;
            _timeBeforeNextCharge = 0;
            _scanhitShowTime = 0;
            if (_scanhitView != null)
                _scanhitPool.ReturnToPool(_scanhitView);
        }

        public void Update(float deltaTime)
        {
            if (_chargesLeft < _weaponStatSo.MaxRecharges)
            {
                _timeBeforeNextCharge += deltaTime;
                if (_timeBeforeNextCharge >= _weaponStatSo.TimeToRecharge)
                {
                    _chargesLeft = Math.Min(_chargesLeft + 1, _weaponStatSo.MaxRecharges);
                    _timeBeforeNextCharge = 0;
                }
            }

            if (_scanhitShowTime <= 0) return;
            _scanhitShowTime -= deltaTime;
            if (_scanhitShowTime <= 0)
            {
                _scanhitView.Enable(false);
            }
        }

        public float GetCooldown()
        {
            return Mathf.Max(0, _weaponStatSo.TimeToRecharge - _timeBeforeNextCharge);
        }

        public uint GetMagazineSize()
        {
            return _chargesLeft;
        }
    }
}