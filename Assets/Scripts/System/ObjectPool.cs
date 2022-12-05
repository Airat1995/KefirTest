using System;
using System.Collections.Generic;
using KefirTest.View;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KefirTest.System
{
    public class ObjectPool<T> where T : GameEntity
    {
        private readonly T _original;
        private List<T> _entities;
        private readonly Action<T> _initObjectAction;

        public ObjectPool(T original, uint count, Action<T> initObjectAction)
        {
            _original = original;
            _entities = new List<T>((int) count);
            _initObjectAction = initObjectAction;
        }

        public T GetEntity()
        {
            T requestedObject;
            if (_entities.Count > 0)
            {
                requestedObject = _entities[_entities.Count - 1];
                _entities.RemoveAt(_entities.Count - 1);
                requestedObject.Enable(true);
            }
            else
            {
                requestedObject = Object.Instantiate(_original, Vector3.zero, Quaternion.identity);
                _initObjectAction(requestedObject);
            }

            return requestedObject;
        }
        
        public void ReturnToPool(T poolObject)
        {
            _entities.Add(poolObject);
            poolObject.Enable(false);
        }
    }
}