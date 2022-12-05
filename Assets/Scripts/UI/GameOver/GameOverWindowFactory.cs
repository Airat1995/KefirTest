using KefirTest.ResourceManager.Interface;
using KefirTest.System.Interface;
using UI.Interface;
using UnityEngine;

namespace UI.GameOver
{
    public class GameOverWindowFactory : IWindowFactory<GameOverWindowController>
    {
        private readonly IWindowResourceManager _resourceManager;
        private readonly IRessetable _ressetable;
        private readonly IPlayerScoreGetter _scoreGetter;
        private readonly Canvas _canvas;

        public GameOverWindowFactory(Canvas canvas, IWindowResourceManager resourceManager, IRessetable ressetable, IPlayerScoreGetter scoreGetter)
        {
            _resourceManager = resourceManager;
            _ressetable = ressetable;
            _scoreGetter = scoreGetter;
            _canvas = canvas;
        }
        
        public GameOverWindowController CreateWindow()
        {
            GameOverWindowView view = _resourceManager.GetWindowView<GameOverWindowView>(_canvas.transform);
            view.transform.SetParent(_canvas.transform);
            GameOverPresenter presenter = new GameOverPresenter(view, _scoreGetter, _ressetable);
            GameOverWindowController gameOverWindowController = new GameOverWindowController(presenter);

            return gameOverWindowController;
        }
    }
}