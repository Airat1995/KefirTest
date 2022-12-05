using System;
using System.Collections.Generic;
using KefirTest.Data;
using KefirTest.System.Interface;
using KefirTest.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KefirTest.System
{
    public class UFOControlSystem : IEnemyControlSystem
    {
        //Constant data
        private readonly Dictionary<Enemy, EnemyDescriptor> _enemyDescriptors;
        private readonly IMovableElement _followElement;
        private readonly Bounds _cameraBounds;
        private readonly Dictionary<Enemy, ObjectScreenTeleporter> _ufosTeleporter;

        //Resettable data
        private readonly Dictionary<Enemy, EnemyStatSO> _activeEnemies;
        private readonly Dictionary<Type, ObjectPool<Enemy>> _enemyPool;
        private readonly Dictionary<Enemy, Timer> _timeToSpawn;

        public UFOControlSystem(Dictionary<Enemy, EnemyDescriptor> enemyDescriptors, IMovableElement followElement, Bounds cameraBounds)
        {
            _enemyDescriptors = enemyDescriptors;
            _followElement = followElement;
            _cameraBounds = cameraBounds;
            _enemyPool = new Dictionary<Type, ObjectPool<Enemy>>();
            _ufosTeleporter = new Dictionary<Enemy, ObjectScreenTeleporter>();
            _activeEnemies = new Dictionary<Enemy, EnemyStatSO>();
            _timeToSpawn = new Dictionary<Enemy, Timer>();
            
            foreach (var enemyDescriptor in enemyDescriptors)
            {
                Type enemyVisualType = enemyDescriptor.Key.GetType();
                if(_enemyPool.ContainsKey(enemyVisualType))
                    continue;
                
                var ufoPool = new ObjectPool<Enemy>(enemyDescriptor.Key,
                    enemyDescriptor.Value.LevelEnemySpawnInfo.PoolSize, (ufo) =>
                    {
                        ObjectScreenTeleporter objectScreenTeleporter =
                            new ObjectScreenTeleporter(ufo, cameraBounds);
                        _ufosTeleporter.Add(ufo, objectScreenTeleporter);
                    });
                
                _enemyPool.Add(enemyVisualType, ufoPool);
                if (enemyDescriptor.Value.Spawnable)
                {
                    _timeToSpawn.Add(enemyDescriptor.Key, new Timer()
                    {
                        TimeLeft = enemyDescriptor.Value.LevelEnemySpawnInfo.RespawnTimer,
                        MaxTime = enemyDescriptor.Value.LevelEnemySpawnInfo.RespawnTimer
                    });
                }
            }

        }

        private void SpawnEnemy(Enemy enemy)
        {
            Enemy spawnedEnemy = _enemyPool[enemy.GetType()].GetEntity();
            spawnedEnemy.SetPosition(GetRandomPositionOutsideCamera());
            _activeEnemies.Add(spawnedEnemy, _enemyDescriptors[enemy].EnemyData);
        }
        
        public bool DestroyEnemy(Enemy enemy, bool completely)
        {
            if (!_activeEnemies.ContainsKey(enemy)) return false;
            _activeEnemies.Remove(enemy);
            _enemyPool[enemy.GetType()].ReturnToPool(enemy);

            return true;
        }

        public void FixedUpdate(float deltaTime)
        {
            foreach (var activeEnemyStatSo in _activeEnemies)
            {
                Enemy activeEnemy = activeEnemyStatSo.Key;
                Vector3 moveVector = (_followElement.GetPosition() - activeEnemy.GetPosition()).normalized;
                activeEnemy.AddPosition(moveVector * (activeEnemyStatSo.Value.Speed * deltaTime));
            }
        }
        
        public void Update(float deltaTime)
        {
            foreach (var screenTeleporter in _ufosTeleporter)
            {
                screenTeleporter.Value.Update(deltaTime);
            }

            foreach (var key in _timeToSpawn.Keys)
            {
                _timeToSpawn[key].TimeLeft -= deltaTime;
                if(_timeToSpawn[key].TimeLeft > 0)
                    continue;
                SpawnEnemy(key);
                _timeToSpawn[key].TimeLeft = _timeToSpawn[key].MaxTime;
            }
        }
        
        private Vector2 GetRandomPositionOutsideCamera()
        {
            float randX = _cameraBounds.size.x * Mathf.Sqrt(Random.Range(0.0f, 1.0f));
            float randY = _cameraBounds.size.y * Mathf.Sqrt(Random.Range(0.0f, 1.0f));
            float theta = Random.Range(0, 1) * 2 * Mathf.PI;

            return new Vector2(_cameraBounds.center.x + randX * Mathf.Cos(theta),
                _cameraBounds.center.y + randY * Mathf.Sin(theta));
        }
        
        public void Reset()
        {
            foreach (var activeEnemy in _activeEnemies)
            {
                _enemyPool[activeEnemy.Key.GetType()].ReturnToPool(activeEnemy.Key);
            }
            _activeEnemies.Clear();
            foreach (var key in _timeToSpawn.Keys)
            {
                _timeToSpawn[key].TimeLeft = _timeToSpawn[key].MaxTime;
            }
        }
    }
}