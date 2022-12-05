namespace UI.Interface
{
    public interface IWindowFactory<T> where T : IWindowController
    {
        T CreateWindow();
    }
}