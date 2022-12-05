using UnityEngine;

namespace KefirTest.Data
{
    [CreateAssetMenu(menuName = "Create LevelEnemySpawnInfo", fileName = "LevelEnemySpawnInfo", order = 0)]
    public class LevelEnemySpawnInfo : ScriptableObject
    {
        [SerializeField]
        private EnemyStatSO _stat;
        public EnemyStatSO StatSo => _stat;

        [SerializeField]
        private uint _poolSize;
        public uint PoolSize => _poolSize;

        [SerializeField]
        private float _respawnTimer;
        public float RespawnTimer => _respawnTimer;
    }
}