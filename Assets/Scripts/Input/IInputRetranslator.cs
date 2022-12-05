using System;
using UnityEngine;

namespace KefirTest.Input
{
    public interface IInputRetranslator
    {
        void SubscribeOnMove(Action<bool> onMovement);

        void SubscribeOnRotate(Action<Vector2> onRotate);

        void SubscribeOnFire(Action<bool> onFire);

        void SubscribeOnAltFire(Action onAltFire);
    }
}