using UI.Interface.GameOver;

namespace UI.GameOver
{
    public class GameOverWindowController : IGameOverWindowController
    {
        private readonly IGameOverPresenter _gameOverPresenter;

        public GameOverWindowController(IGameOverPresenter gameOverPresenter)
        {
            _gameOverPresenter = gameOverPresenter;
        }
        
        public void Open()
        {
            _gameOverPresenter.Open();
        }

        public void Close()
        {
            _gameOverPresenter.Close();
        }

        public void Update(float deltaTime)
        {
        }
    }
}