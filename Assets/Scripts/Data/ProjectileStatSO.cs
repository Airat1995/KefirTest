using UnityEngine;

namespace KefirTest.Data
{
    [CreateAssetMenu(menuName = "Create Projectile", fileName = "Projectile", order = 2)]
    public class ProjectileStatSO : ScriptableObject
    {
        [SerializeField]
        private float _speed;
        public float Speed => _speed;

        [SerializeField]
        private float _liveTime;
        public float LiveTime => _liveTime;
    }
}