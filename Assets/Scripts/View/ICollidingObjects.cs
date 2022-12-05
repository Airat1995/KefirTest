using System;
using UnityEngine;

namespace KefirTest.View
{
    public interface ICollidingObjects
    {
        void SubscribeOnCollide(Action<Collider2D, GameEntity> onCollisionAction);
    }
}