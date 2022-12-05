using UnityEngine;

namespace KefirTest.Data
{
    [CreateAssetMenu(menuName = "Create LazerWeaponStat", fileName = "LazerWeaponStat", order = 1)]
    public class ScanHitWeaponStatSO : ScriptableObject
    {
        [SerializeField]
        private float _timeToRecharge;
        public float TimeToRecharge => _timeToRecharge;

        [SerializeField]
        private uint _maxRecharges;
        public uint MaxRecharges => _maxRecharges;

        [SerializeField]
        private float _visibilityTime;
        public float VisibilityTime => _visibilityTime;
    }
}