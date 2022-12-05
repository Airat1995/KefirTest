using UnityEngine;

namespace KefirTest.Data
{
    [CreateAssetMenu(menuName = "Create AsteroidStat", fileName = "Asteroid", order = 3)]
    public class AsteroidStatSO : EnemyStatSO
    {
        [SerializeField]
        private AsteroidStatSO[] _asteroidOnDestroy;
        public AsteroidStatSO[] AsteroidStatSo => _asteroidOnDestroy;
    }
}