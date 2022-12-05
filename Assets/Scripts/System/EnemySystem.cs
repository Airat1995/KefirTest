using System;
using System.Collections.Generic;
using KefirTest.Data;
using KefirTest.System.Interface;
using KefirTest.View;
using UnityEngine;

namespace KefirTest.System
{
    public class EnemySystem : IUpdatableSystem, IFixedUpdatableSystem, IEnemyDestroyNotifier, IResettableSystem
    {
        private readonly IPlayerScoreNotifier _playerScoreNotifier;
        private readonly Dictionary<Type, IEnemyControlSystem> _enemyControlSystems;

        public EnemySystem(IPlayerScoreNotifier playerScoreNotifier, List<EnemyDescriptor> enemiesDescriptors, IMovableElement followElement, Bounds cameraBoudns)
        {
            _enemyControlSystems = new Dictionary<Type, IEnemyControlSystem>();
            _playerScoreNotifier = playerScoreNotifier;
            foreach (var levelEnemySpawnInfo in enemiesDescriptors)
            {
                if(!levelEnemySpawnInfo.Spawnable)
                    continue;
                IEnemyControlSystem enemyControlSystem = null;
                switch (levelEnemySpawnInfo.EnemyData)
                {
                    case AsteroidStatSO:
                        Dictionary<Enemy, EnemyDescriptor> enemyAsteroidsDescriptorsDict = CreateDescriptorsDictForEnemy(enemiesDescriptors, typeof(AsteroidStatSO));
                        enemyControlSystem =
                            new AsteroidControlSystem(enemyAsteroidsDescriptorsDict, cameraBoudns);
                        break;
                    case UFOStatSO:
                        Dictionary<Enemy, EnemyDescriptor> enemyUFODescriptorsDict = CreateDescriptorsDictForEnemy(enemiesDescriptors, typeof(UFOStatSO));
                        enemyControlSystem =
                            new UFOControlSystem(enemyUFODescriptorsDict, followElement, cameraBoudns);
                        break;
                }
                if(enemyControlSystem == null)
                    continue;
                _enemyControlSystems.Add(levelEnemySpawnInfo.AssignedVisual.GetType(), enemyControlSystem);
            }
        }

        private Dictionary<Enemy, EnemyDescriptor> CreateDescriptorsDictForEnemy(List<EnemyDescriptor> enemiesDescriptors, Type enemyType)
        {
            Dictionary<Enemy, EnemyDescriptor> enemyDict = new Dictionary<Enemy, EnemyDescriptor>();
            foreach (var enemyDescriptor in enemiesDescriptors)
            {
                if(enemyDict.ContainsKey(enemyDescriptor.AssignedVisual) || enemyDescriptor.EnemyData.GetType() != enemyType)
                    continue;
                enemyDict.Add(enemyDescriptor.AssignedVisual, enemyDescriptor);
            }

            return enemyDict;
        }

        public void FixedUpdate(float deltaTime)
        {
            foreach (var enemyControlSystem in _enemyControlSystems)
            {
                enemyControlSystem.Value.FixedUpdate(deltaTime);
            }
        }
        
        public void Update(float deltaTime)
        {
            foreach (var enemyControlSystem in _enemyControlSystems)
            {
                enemyControlSystem.Value.Update(deltaTime);
            }
        }

        public void EnemyDestroy(Enemy enemy, bool completely)
        {
            _playerScoreNotifier.AddScore(enemy);
            foreach (var enemyControlSystem in _enemyControlSystems)
            {
                if(enemyControlSystem.Value.DestroyEnemy(enemy, completely))
                    break;
            }
        }

        public void Reset()
        {
            foreach (var enemyControlSystem in _enemyControlSystems)
            {
                enemyControlSystem.Value.Reset();
            }
        }
    }
}