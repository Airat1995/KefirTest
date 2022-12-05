using UnityEngine;

namespace KefirTest.Data
{
    [CreateAssetMenu(menuName = "Create ProjectileWeaponStatSO", fileName = "ProjectileWeaponStat", order = 2)]
    public class ProjectileWeaponStatSO : ScriptableObject
    {
        [SerializeField]
        private float _cooldownTime;
        public float CooldownTime => _cooldownTime;

        [SerializeField]
        private uint _projectilesPoolCount;
        public uint ProjectilesPoolCount => _projectilesPoolCount;

        [SerializeField]
        private ProjectileStatSO projectileStatSo;
        public ProjectileStatSO ProjectileStatSo => projectileStatSo;
    }
}