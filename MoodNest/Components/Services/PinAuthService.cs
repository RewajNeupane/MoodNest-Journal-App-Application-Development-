using System;
using System.Linq;
using MoodNest.Data;
using MoodNest.Entities;

namespace MoodNest.Components.Services
{
    public class PinAuthService
    {
        private readonly AppDbContext _db;

        public int? CurrentUserId { get; private set; }
        public bool IsUnlocked => CurrentUserId != null;

        public event Action? OnAuthChanged;

        public PinAuthService(AppDbContext db)
        {
            _db = db;
        }

        /* =======================
           LOGIN
        ======================== */

        // 🔐 Login with Username OR Email + PIN
        public bool VerifyPin(string identifier, string pin)
        {
            var user = _db.Users.FirstOrDefault(u =>
                u.Username == identifier || u.Email == identifier);

            if (user == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(pin, user.PinHash))
                return false;

            CurrentUserId = user.Id;
            OnAuthChanged?.Invoke();
            return true;
        }

        /* =======================
           LOGOUT / LOCK
        ======================== */

        // 🔒 Lock app
        public void Lock()
        {
            CurrentUserId = null;
            OnAuthChanged?.Invoke();
        }

        /* =======================
           CURRENT USER
        ======================== */

        // ✅ Get logged-in user
        public User? GetCurrentUser()
        {
            if (CurrentUserId == null)
                return null;

            return _db.Users.Find(CurrentUserId.Value);
        }

        // ✅ Update username + email
        public bool UpdateProfile(string username, string email)
        {
            if (CurrentUserId == null)
                return false;

            var user = _db.Users.Find(CurrentUserId.Value);
            if (user == null)
                return false;

            user.Username = username.Trim();
            user.Email = email.Trim();

            _db.SaveChanges();
            return true;
        }

        /* =======================
           PIN
        ======================== */

        // 🔁 Change PIN for current user
        public bool ChangePin(string currentPin, string newPin)
        {
            if (CurrentUserId == null)
                return false;

            var user = _db.Users.Find(CurrentUserId.Value);
            if (user == null)
                return false;

            if (!BCrypt.Net.BCrypt.Verify(currentPin, user.PinHash))
                return false;

            user.PinHash = BCrypt.Net.BCrypt.HashPassword(newPin);
            _db.SaveChanges();

            return true;
        }

        /* =======================
           ROUTE GUARD
        ======================== */

        // ✅ Route access rules
        public bool CanAccess(string route)
        {
            // Allow register + login without auth
            if (route.StartsWith("register") || route == "")
                return true;

            return IsUnlocked;
        }
    }
}
