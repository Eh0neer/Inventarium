using System.Security.Cryptography;
using System.Text;
using Inventarium.Context;
using Inventarium.Interfases;
using Inventarium.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventarium.Services
{
    public class UserService : IUser
    {
        private readonly MyDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(MyDbContext dbContext, ILogger<UserService> logger)
        {
            _context = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Service to add a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if user added successfully, otherwise false</returns>
        public async Task<bool> AddUserAsync(User users, string password)
        {
            try
            {
                // Генерация соли для нового пользователя
                byte[] salt = GenerateSalt();
                // Хеширование пароля пользователя с использованием соли
                string hashedPassword = HashPassword(password, salt);

                // Создание нового объекта пользователя и заполнение его данными
                var user = new User
                {
                    Username = users.Username,
                    Salt = Convert.ToBase64String(salt),
                    Password = hashedPassword
                };

                // Сохранение пользователя в базе данных
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User {Username} added successfully", users.Username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding user {Username}", users.Username);
                return false; // Indicates failure
            }
        }

        
        // Метод для генерации случайной соли
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Метод для хеширования пароля с использованием соли
        private string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

                byte[] hashedBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }


        /// <summary>
        /// Service to remove a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if user removed successfully, otherwise false</returns>
        public async Task<bool> RemoveUserAsync(User user)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("User: {Username} removed successfully", user.Username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when deleting a user: {Username}", user.Username);
                return false; // Indicates failure
            }
        }
        
        /// <summary>
        /// Service to update a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if user updated successfully, otherwise false</returns>
        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                _context.Users.Update(user); // Исправлено на передачу объекта user
                await _context.SaveChangesAsync();
                _logger.LogInformation("User: {Username} updated successfully", user.Username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred when updating a user: {Username}", user.Username);
                return false; // Indicates failure
            }
        }

        
        /// <summary>
        /// Service to retrieve username
        /// </summary>
        /// <param name="user">Username to retrieve</param>
        /// <returns>User name received, otherwise null</returns>
        public async Task<User> GetUserAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == user.UserId);
                
                if (existingUser != null)
                {
                    _logger.LogInformation("User found: {Username}", existingUser.Username);
                    return existingUser;
                }
                else
                {
                    _logger.LogWarning("User not found for Id: {UserId}", user.UserId);
                    return null!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user");
                return null!;
            }
        }

    }
}
