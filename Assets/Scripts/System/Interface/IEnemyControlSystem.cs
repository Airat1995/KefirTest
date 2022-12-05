using KefirTest.View;

namespace KefirTest.System.Interface
{
    public interface IEnemyControlSystem : IResettableSystem
    {
        bool DestroyEnemy(Enemy enemy, bool completely);

        void Update(float deltaTime);

        void FixedUpdate(float deltaTime);
    }
}