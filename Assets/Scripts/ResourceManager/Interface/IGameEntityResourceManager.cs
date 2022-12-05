using KefirTest.View;

namespace KefirTest.ResourceManager.Interface
{
    public interface IGameEntityResourceManager
    {
        T GetEntity<T>(string name) where T : GameEntity;
    }
}