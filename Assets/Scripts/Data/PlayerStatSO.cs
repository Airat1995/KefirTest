using UnityEngine;


namespace KefirTest.Data
{
    [CreateAssetMenu(menuName = "Create PlayerStats", fileName = "PlayerStats", order = 0)]
    public class PlayerStatSO : ScriptableObject
    {
        [SerializeField]
        private float _accelerationSpeed;
        public float AccelerationSpeed => _accelerationSpeed;
        
        [SerializeField]
        private float maxSpeed;
        public float MaxSpeed => maxSpeed;

        [SerializeField]
        private float _deaccelerationSpeed;
        public float DeaccelerationSpeed => _deaccelerationSpeed;

        [SerializeField]
        private float _rotationSpeed;
        public float RotationSpeed => _rotationSpeed;
        
        [SerializeField]
        private ScanHitWeaponStatSO scanHitWeapon;
        public ScanHitWeaponStatSO ScanHitWeapon => scanHitWeapon;

        [SerializeField]
        private ProjectileWeaponStatSO _projectileWeapon;
        public ProjectileWeaponStatSO ProjectileWeapon => _projectileWeapon;
    }
}