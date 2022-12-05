using UnityEngine;

namespace KefirTest.View
{
    public interface IMovableElement
    {
        void SetPosition(Vector3 newPosition);

        void SetRotation(Quaternion newRotation);

        void AddPosition(Vector3 position);

        void AddRotation(float rotation);

        Vector3 GetPosition();

        Quaternion GetRotation();
        
        Vector3 GetForwardVector();

        Vector2 GetSize();
    }
}