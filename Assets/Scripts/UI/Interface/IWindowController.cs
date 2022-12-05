namespace UI.Interface
{
    public interface IWindowController
    {
        void Open();

        void Close();
        
        void Update(float deltaTime);
    }
}