using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KefirTest.Input
{
    public class InputReceiver : MonoBehaviour, IInputRetranslator
    {
        private Action<bool> _onMove = (_) => { };
        private Action<Vector2> _onRotate = (_) => { };
        private Action<bool> _onFire = (_) => { };
        private Action _onAltFire = () => { };

        public void Move(InputAction.CallbackContext context)
        {
            _onMove(context.canceled);
        }

        public void Rotate(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>() * -1;
            _onRotate(input);
        }

        public void Fire(InputAction.CallbackContext context)
        {
            _onFire(context.canceled);
        }

        public void AltFire(InputAction.CallbackContext context)
        {
            if (context.performed)
                _onAltFire();
        }

        public void SubscribeOnMove(Action<bool> onMovement)
        {
            _onMove += onMovement;
        }

        public void SubscribeOnRotate(Action<Vector2> onRotate)
        {
            _onRotate += onRotate;
        }

        public void SubscribeOnFire(Action<bool> onFire)
        {
            _onFire += onFire;
        }

        public void SubscribeOnAltFire(Action onAltFire)
        {
            _onAltFire += onAltFire;
        }
    }
}
