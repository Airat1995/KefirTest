using KefirTest.UI.Interface;

namespace UI.Interface.GameOver
{
    public interface IGameOverView : IWindowView
    {
        void ShowScore(float score);

        void SubscribeEventReceiver(IGameOverEventReceiver eventReceiver);
    }
}