using System;
using KefirTest.System.Interface;
using KefirTest.View;
using UnityEngine;

namespace KefirTest.System
{
    public class PlayerDamageSystem
    {
        private readonly IPausable _pausable;
        private readonly Action _gameStopAction;

        public PlayerDamageSystem(Player player, IPausable pausable)
        {
            _pausable = pausable;

            player.SubscribeOnCollide(EnemyCollide);
        }

        private void EnemyCollide(Collider2D obj, GameEntity gameEntity)
        {
            _pausable.Pause(true);
        }
    }
}