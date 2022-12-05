using KefirTest.System.Interface;
using UI.Interface.GameOver;

namespace UI.GameOver
{
    public class GameOverPresenter : IGameOverPresenter, IGameOverEventReceiver
    {
        private readonly GameOverWindowView _windowView;
        private readonly IPlayerScoreGetter _playerScoreGetter;
        private readonly IRessetable _ressetable;

        public GameOverPresenter(GameOverWindowView windowView, IPlayerScoreGetter playerScoreGetter, IRessetable ressetable)
        {
            _windowView = windowView;
            _playerScoreGetter = playerScoreGetter;
            _ressetable = ressetable;
            _windowView.SubscribeEventReceiver(this);
        }
        
        public void Open()
        {
            _windowView.ShowScore(_playerScoreGetter.GetScore());
            _windowView.Open();
        }

        public void Close()
        {
            _windowView.Close();
        }

        public void Reset()
        {
            _ressetable.Reset();
        }
    }
}