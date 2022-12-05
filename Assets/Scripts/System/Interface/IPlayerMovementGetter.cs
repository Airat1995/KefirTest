using UnityEngine;

namespace KefirTest.System.Interface
{
    public interface IPlayerMovementGetter
    {
        Vector2 GetPosition();

        float GetRotation();

        float GetSpeed();
    }
}