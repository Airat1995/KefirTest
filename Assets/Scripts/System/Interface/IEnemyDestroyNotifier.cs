using KefirTest.View;

namespace KefirTest.System.Interface
{
    public interface IEnemyDestroyNotifier
    {
        void EnemyDestroy(Enemy enemy, bool completely);
    }
}