using Inventarium.Models;

namespace Inventarium.Interfases;

public interface IUser
{
    Task<bool> AddUserAsync(User user, string password);
    Task<bool> RemoveUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<User> GetUserAsync(User user);
}