using System;
using KefirTest.Data;
using KefirTest.View;
using UnityEngine;

namespace KefirTest.System
{
    [Serializable]
    public class EnemyDescriptor
    {

        [SerializeField]
        private EnemyStatSO _enemyData;
        public EnemyStatSO EnemyData => _enemyData;

        [SerializeField]
        private string _assignedVisualName;
        public string AssignedVisualName => _assignedVisualName;
        
        public Enemy AssignedVisual { get; set; }

        [SerializeField]
        private LevelEnemySpawnInfo _spawnInfo;
        public LevelEnemySpawnInfo LevelEnemySpawnInfo => _spawnInfo;

        [SerializeField]
        private bool _spawnable;
        public bool Spawnable => _spawnable;
    }
}