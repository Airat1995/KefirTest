using KefirTest.Data;
using KefirTest.Input;
using KefirTest.System.Interface;
using KefirTest.View;
using UnityEngine;

namespace KefirTest.System
{
    public class PlayerMovementSystem : IFixedUpdatableSystem, IResettableSystem, IPlayerMovementGetter
    {
        private readonly IMovableElement _player;
        private readonly PlayerStatSO _playerStatSo;
        private readonly IInputRetranslator _inputRetranslator;
        private readonly Vector3 _playerStartPosition;
        private readonly Vector3 _playerStartRotation;

        #region resettable data
        private float _speed;
        private Vector2 _rotation;
        private bool _movementStopped = true;
        #endregion

        public PlayerMovementSystem(IMovableElement player, PlayerStatSO playerStatSo,
            IInputRetranslator inputRetranslator, Vector3 playerStartPosition, Vector3 playerStartRotation)
        {
            _player = player;
            _playerStatSo = playerStatSo;
            _inputRetranslator = inputRetranslator;
            _playerStartPosition = playerStartPosition;
            _playerStartRotation = playerStartRotation;

            _inputRetranslator.SubscribeOnMove(Move);
            _inputRetranslator.SubscribeOnRotate(Rotate);
            _player.SetPosition(playerStartPosition);
            _player.SetRotation(Quaternion.Euler(playerStartRotation));
        }

        private void Move(bool canceled)
        {
            _movementStopped = canceled;
        }

        private void Rotate(Vector2 rotate)
        {
            _rotation = rotate;
        }

        public void FixedUpdate(float deltaTime)
        {
            _player.AddPosition(_player.GetForwardVector() * (_speed * deltaTime));
            if (!_movementStopped)
            {
                _speed += _playerStatSo.AccelerationSpeed * deltaTime;
                _speed = Mathf.Min(_playerStatSo.MaxSpeed, _speed);
            }
            else
            {
                _speed -= _playerStatSo.DeaccelerationSpeed * deltaTime;
                _speed = Mathf.Max(0, _speed);
            }

            if (_rotation != Vector2.zero)
            {
                _player.AddRotation(_rotation.x);
            }
        }

        public void Reset()
        {
            _player.SetPosition(_playerStartPosition);
            _player.SetRotation(Quaternion.Euler(_playerStartRotation));
            _movementStopped = true;
            _speed = 0;
            _rotation = Vector2.zero;
        }

        public Vector2 GetPosition()
        {
            return _player.GetPosition();
        }

        public float GetRotation()
        {
            return _player.GetRotation().eulerAngles.z;
        }

        public float GetSpeed()
        {
            return _speed;
        }
    }
}
