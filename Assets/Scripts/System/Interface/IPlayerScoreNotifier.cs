using KefirTest.View;

namespace KefirTest.System.Interface
{
    public interface IPlayerScoreNotifier
    {
        void AddScore(Enemy destroyedEnemy);
    }
}