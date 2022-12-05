using UnityEngine;

namespace KefirTest.Data
{
    public class EnemyStatSO : ScriptableObject
    {
        [SerializeField]
        private float _destroyScore;
        public float DestroyScore => _destroyScore;
        
        [SerializeField]
        private float _speed;
        public float Speed => _speed;
    }
}