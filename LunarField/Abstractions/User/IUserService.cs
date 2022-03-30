using LunarField.Models.User;

namespace LunarField.Abstractions.User;

public interface IUserService
{
    Task<UserOutput> CreateUserAsync(string userName, string password, string fullName, DateTime dateYear);
}