using KefirTest.UI.Interface.Main;

namespace KefirTest.UI.Main
{
    public class MainWindowController : IMainWindowController
    {
        private readonly IMainWindowPresenter _presenter;

        public MainWindowController(IMainWindowPresenter presenter)
        {
            _presenter = presenter;
        }
        
        public void Open()
        {
            _presenter.Open();
        }

        public void Close()
        {
            _presenter.Close();
        }

        public void Update(float deltaTime)
        {
            _presenter.Update();
        }
    }
}