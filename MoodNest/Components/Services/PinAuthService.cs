using System;
using System.Linq;
using MoodNest.Data;
using MoodNest.Entities;

namespace MoodNest.Components.Services
{
    /// <summary>
    /// Handles PIN-based authentication, user session state,
    /// and basic profile/security operations.
    /// </summary>
    public class PinAuthService
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Currently authenticated user's ID.
        /// Null indicates the application is locked.
        /// </summary>
        public int? CurrentUserId { get; private set; }

        /// <summary>
        /// Indicates whether the application is unlocked.
        /// </summary>
        public bool IsUnlocked => CurrentUserId != null;

        /// <summary>
        /// Event triggered whenever authentication state changes
        /// (login or logout).
        /// </summary>
        public event Action? OnAuthChanged;

        public PinAuthService(AppDbContext db)
        {
            _db = db;
        }

        /* =======================
           LOGIN
        ======================== */

        /// <summary>
        /// Verifies user credentials using either username or email
        /// combined with a 4-digit PIN.
        /// </summary>
        /// <param name="identifier">Username or email</param>
        /// <param name="pin">Plain-text PIN entered by the user</param>
        /// <returns>True if authentication succeeds</returns>
        public bool VerifyPin(string identifier, string pin)
        {
            var user = _db.Users.FirstOrDefault(u =>
                u.Username == identifier || u.Email == identifier);

            if (user == null)
                return false;

            // Secure PIN verification using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(pin, user.PinHash))
                return false;

            CurrentUserId = user.Id;
            OnAuthChanged?.Invoke();
            return true;
        }

        /* =======================
           LOGOUT / LOCK
        ======================== */

        /// <summary>
        /// Locks the application and clears authentication state.
        /// </summary>
        public void Lock()
        {
            CurrentUserId = null;
            OnAuthChanged?.Invoke();
        }

        /* =======================
           CURRENT USER
        ======================== */

        /// <summary>
        /// Retrieves the currently authenticated user entity.
        /// </summary>
        /// <returns>User entity or null if not authenticated</returns>
        public User? GetCurrentUser()
        {
            if (CurrentUserId == null)
                return null;

            return _db.Users.Find(CurrentUserId.Value);
        }

        /// <summary>
        /// Updates the username and email of the current user.
        /// </summary>
        /// <param name="username">New username</param>
        /// <param name="email">New email address</param>
        /// <returns>True if update succeeds</returns>
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
           PIN MANAGEMENT
        ======================== */

        /// <summary>
        /// Changes the PIN of the currently authenticated user.
        /// </summary>
        /// <param name="currentPin">Existing PIN</param>
        /// <param name="newPin">New PIN</param>
        /// <returns>True if PIN change succeeds</returns>
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

        /// <summary>
        /// Determines whether a given route can be accessed
        /// based on authentication state.
        /// </summary>
        /// <param name="route">Route name</param>
        /// <returns>True if access is allowed</returns>
        public bool CanAccess(string route)
        {
            // Allow unauthenticated access to login and registration
            if (route.StartsWith("register") || route == "")
                return true;

            return IsUnlocked;
        }
    }
}
