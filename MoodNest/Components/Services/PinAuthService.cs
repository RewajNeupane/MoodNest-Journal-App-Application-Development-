namespace MoodNest.Components.Services
{
    public class PinAuthService
    {
        public bool IsUnlocked { get; private set; } = false;

        public void Unlock()
        {
            IsUnlocked = true;
        }

        public void Lock()
        {
            IsUnlocked = false;
        }
    }
}