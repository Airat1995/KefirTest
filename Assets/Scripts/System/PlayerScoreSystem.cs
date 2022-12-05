using System;
using System.Collections.Generic;
using KefirTest.Data;
using KefirTest.System.Interface;
using KefirTest.View;

namespace KefirTest.System
{
    public class PlayerScoreSystem : IPlayerScoreNotifier, IPlayerScoreGetter, IResettableSystem
    {
        private readonly Dictionary<Type, EnemyStatSO> _enemiesSats;

        private float _currentScore;

        public PlayerScoreSystem(Dictionary<Type, EnemyStatSO> enemiesSats)
        {
            _enemiesSats = enemiesSats;
            _currentScore = 0;
        }

        public float GetScore() => _currentScore;

        public void AddScore(Enemy destroyedEnemy)
        {
            _currentScore += _enemiesSats[destroyedEnemy.GetType()].DestroyScore;
        }

        public void Reset()
        {
            _currentScore = 0;
        }
    }
}