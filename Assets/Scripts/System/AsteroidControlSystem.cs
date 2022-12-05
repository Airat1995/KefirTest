using System;
using System.Collections.Generic;
using KefirTest.Data;
using KefirTest.System.Interface;
using KefirTest.View;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KefirTest.System
{
    public class AsteroidControlSystem : IEnemyControlSystem
    {
        #region constant data
        private readonly Dictionary<Enemy, EnemyDescriptor> _enemyDescriptors;
        private readonly Bounds _cameraBounds;
        private readonly Dictionary<Enemy, ObjectScreenTeleporter> _asteroidsTeleporter;
        #endregion

        #region resettable data
        private readonly Dictionary<Type, ObjectPool<Enemy>> _enemyPool;
        private readonly Dictionary<Enemy, EnemyStatSO> _activeEnemies;
        private readonly Dictionary<Enemy, Timer> _timeToSpawn;
        #endregion

        public AsteroidControlSystem(Dictionary<Enemy, EnemyDescriptor> enemyDescriptors, Bounds cameraBounds)
        {
            _enemyDescriptors = enemyDescriptors;
            _cameraBounds = cameraBounds;
            _enemyPool = new Dictionary<Type, ObjectPool<Enemy>>();
            _asteroidsTeleporter = new Dictionary<Enemy, ObjectScreenTeleporter>();
            _activeEnemies = new Dictionary<Enemy, EnemyStatSO>();
            _timeToSpawn = new Dictionary<Enemy, Timer>();
            
            foreach (var enemyDescriptor in enemyDescriptors)
            {
                Type enemyVisualType = enemyDescriptor.Key.GetType();
                if(_enemyPool.ContainsKey(enemyVisualType))
                    continue;
                
                var asteroidPool = new ObjectPool<Enemy>(enemyDescriptor.Key,
                    enemyDescriptor.Value.LevelEnemySpawnInfo.PoolSize, (asteroid) =>
                    {
                        ObjectScreenTeleporter objectScreenTeleporter =
                            new ObjectScreenTeleporter(asteroid, cameraBounds);
                        _asteroidsTeleporter.Add(asteroid, objectScreenTeleporter);
                    });
                
                _enemyPool.Add(enemyVisualType, asteroidPool);
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

        private void SpawnEnemy(Enemy enemy, Vector2 initialPosition)
        {
            Enemy spawnedEnemy = _enemyPool[enemy.GetType()].GetEntity();
            _activeEnemies.Add(spawnedEnemy, _enemyDescriptors[enemy].EnemyData);

            float angleRand = Random.Range(0, 360);
            spawnedEnemy.SetRotation(Quaternion.Euler(0, 0, angleRand));
            
            spawnedEnemy.SetPosition(initialPosition);
        }
        
        public bool DestroyEnemy(Enemy enemy, bool completely)
        {
            if (!_activeEnemies.ContainsKey(enemy)) return false;
            if (!completely)
            {
                if(_activeEnemies[enemy] is AsteroidStatSO asteroidStat)
                {
                    foreach (var asteroidStatSo in asteroidStat.AsteroidStatSo)
                    {
                        foreach (var descriptor in _enemyDescriptors)
                        {
                            if (descriptor.Value.EnemyData != asteroidStatSo) continue;
                            SpawnEnemy(descriptor.Key, enemy.GetPosition());
                            break;
                        }
                    }
                }
            }
            _activeEnemies.Remove(enemy);
            _enemyPool[enemy.GetType()].ReturnToPool(enemy);

            return true;
        }

        public void FixedUpdate(float deltaTime)
        {
            foreach (var activeEnemyStatSo in _activeEnemies)
            {
                Enemy activeEnemy = activeEnemyStatSo.Key;
                activeEnemy.AddPosition(activeEnemy.GetForwardVector() * (activeEnemyStatSo.Value.Speed * deltaTime));
            }
        }
        
        public void Update(float deltaTime)
        {
            foreach (var screenTeleporter in _asteroidsTeleporter)
            {
                screenTeleporter.Value.Update(deltaTime);
            }

            foreach (var key in _timeToSpawn.Keys)
            {
                _timeToSpawn[key].TimeLeft -= deltaTime;
                if(_timeToSpawn[key].TimeLeft > 0)
                    continue;
                SpawnEnemy(key, GetRandomPositionOutsideCamera());

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