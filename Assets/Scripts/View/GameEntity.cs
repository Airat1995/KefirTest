using System;
using UnityEngine;

namespace KefirTest.View
{
    [RequireComponent(typeof(SpriteRenderer), typeof(Collider2D))]
    public class GameEntity : MonoBehaviour, IMovableElement, ICollidingObjects, IEnabling
    {
        private static readonly Vector3 RotationAngle = new Vector3(0.0f, 0.0f, 1.0f);

        private Action<Collider2D, GameEntity> _onCollision = (_, _) => { };
        
        [SerializeField]
        private Collider2D _collider;

        public void SetPosition(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public void SetRotation(Quaternion newRotation)
        {
            transform.rotation = newRotation;
        }

        public void AddPosition(Vector3 position)
        {
            transform.position += position;
        }

        public void AddRotation(float rotation)
        {
            transform.Rotate(RotationAngle, rotation);
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public Quaternion GetRotation()
        {
            return transform.rotation;
        }

        public Vector3 GetForwardVector()
        {
            return transform.up;
        }

        public Vector2 GetSize()
        {
            return _collider.bounds.size;
        }

        public void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public void SubscribeOnCollide(Action<Collider2D, GameEntity> onCollisionAction)
        {
            _onCollision += onCollisionAction;
        }
        public void ClearSubscription()
        {
            _onCollision = (_, _) => { };
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            _onCollision.Invoke(col, this);
        }
    }
}