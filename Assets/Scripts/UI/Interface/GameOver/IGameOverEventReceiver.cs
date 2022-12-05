using KefirTest.UI.Interface;

namespace UI.Interface.GameOver
{
    public interface IGameOverEventReceiver : IWindowEventReceiver
    {
        void Reset();
    }
}