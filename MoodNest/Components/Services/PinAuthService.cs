using System;
using System.IO;

namespace MoodNest.Components.Services
{
    public class PinAuthService
    {
        private readonly string _pinFilePath;
        private string _pin;

        public bool IsUnlocked { get; private set; }

        public event Action? OnAuthChanged;

        public PinAuthService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dir = Path.Combine(appData, "MoodNest");

            Directory.CreateDirectory(dir);

            _pinFilePath = Path.Combine(dir, "pin.txt");

            _pin = File.Exists(_pinFilePath)
                ? File.ReadAllText(_pinFilePath)
                : "1234";

            SavePin(); // ensure file exists
        }

        public bool VerifyPin(string pin)
        {
            return pin == _pin;
        }

        public bool ChangePin(string currentPin, string newPin)
        {
            if (!VerifyPin(currentPin))
                return false;

            _pin = newPin;
            SavePin();
            return true;
        }

        private void SavePin()
        {
            File.WriteAllText(_pinFilePath, _pin);
        }

        public void Unlock()
        {
            IsUnlocked = true;
            OnAuthChanged?.Invoke();
        }

        public void Lock()
        {
            IsUnlocked = false;
            OnAuthChanged?.Invoke();
        }
    }
}