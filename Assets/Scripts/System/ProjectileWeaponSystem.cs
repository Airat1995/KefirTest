using System;
using System.Collections.Generic;
using KefirTest.Data;
using KefirTest.Input;
using KefirTest.System.Interface;
using KefirTest.View;
using UnityEngine;
using Projectile = KefirTest.View.Projectile;

namespace KefirTest.System
{
    public class ProjectileWeaponSystem : IUpdatableSystem, IResettableSystem
    {
        private readonly IInputRetranslator _inputRetranslator;
        private readonly IEnemyDestroyNotifier _enemyDestroyNotifier;
        private readonly IMovableElement _player;
        private readonly ProjectileWeaponStatSO _weaponStatSo;
        private readonly Dictionary<Projectile, LifeTimeTimer> _activeProjectiles;

        #region resettable data
        private readonly ObjectPool<Projectile> _projectilePool;
        private bool _firing;
        private float _timeBeforeNextShot;
        #endregion

        public ProjectileWeaponSystem(IInputRetranslator inputRetranslator,
            IEnemyDestroyNotifier enemyDestroyNotifier,
            IMovableElement player,
            Projectile projectile,
            ProjectileWeaponStatSO weaponStatSo)
        {
            _inputRetranslator = inputRetranslator;
            _enemyDestroyNotifier = enemyDestroyNotifier;
            _player = player;
            _weaponStatSo = weaponStatSo;
            _projectilePool = new ObjectPool<Projectile>(projectile, _weaponStatSo.ProjectilesPoolCount, InitNewProjectile);
            _activeProjectiles = new Dictionary<Projectile, LifeTimeTimer>((int) _weaponStatSo.ProjectilesPoolCount);
            
            _inputRetranslator.SubscribeOnFire(Fire);
        }

        private void InitNewProjectile(Projectile projectile)
        {
            projectile.SubscribeOnCollide(CollisionCheck);
        }

        private void Fire(bool canceled)
        {
            _firing = !canceled;
        }

        public void Update(float deltaTime)
        {
            _timeBeforeNextShot -= deltaTime;
            RemoveOldProjectiles(deltaTime);
            MoveProjectiles(deltaTime);

            if (!_firing || _timeBeforeNextShot > 0) return;
            _timeBeforeNextShot = _weaponStatSo.CooldownTime;
            SpawnNewProjectile();
        }

        private void SpawnNewProjectile()
        {
            Projectile projectile = _projectilePool.GetEntity();
            projectile.SetPosition(_player.GetPosition() + _player.GetForwardVector());
            projectile.SetRotation(_player.GetRotation());
            _activeProjectiles.Add(projectile, new LifeTimeTimer
            {
                Time = _weaponStatSo.ProjectileStatSo.LiveTime
            });
        }

        private void RemoveOldProjectiles(float deltaTime)
        {
            foreach (var key in _activeProjectiles.Keys)
            {
                _activeProjectiles[key].Time -= deltaTime;
                if(_activeProjectiles[key].Time > 0)
                    continue;
                _projectilePool.ReturnToPool(key);
                _activeProjectiles.Remove(key);
                break;
            }
        }

        private void MoveProjectiles(float deltaTime)
        {
            float speed = _weaponStatSo.ProjectileStatSo.Speed;
            foreach (var projectileKV in _activeProjectiles)
            {
                var projectile = projectileKV.Key;
                projectile.AddPosition(projectile.GetForwardVector() * (deltaTime * speed));
            }
        }

        private void CollisionCheck(Collider2D collider2D)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if(enemy == null)
                return;
            _enemyDestroyNotifier.EnemyDestroy(enemy, false);
        }

        public void Reset()
        {
            _firing = false;
            _timeBeforeNextShot = 0;
            foreach (var projectile in _activeProjectiles)
            {
                _projectilePool.ReturnToPool(projectile.Key);
            }
            _activeProjectiles.Clear();
        }
    }
}