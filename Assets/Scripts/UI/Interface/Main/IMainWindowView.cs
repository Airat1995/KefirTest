namespace KefirTest.UI.Interface.Main
{
    public interface IMainWindowView : IWindowView
    {
        void ShowPosition(float x, float y);
        
        void ShowSpeed(float speed);

        void ShowRotation(float rotation);

        void ShowScanhitWeaponMagazine(uint magazine);

        void ShowScanhitWeaponCooldown(float cooldown);
    }
}