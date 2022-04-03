using LunarField.Models.User;

namespace LunarField.Abstractions.User;

public interface IUserService
{
    Task<UserOutput> CreateUserAsync(string userName, string password, string fullName, DateTime dateYear);

    Task<List<ProfileDataOutput>> GetProfileDataAsync(string userName);

    Task SaveUserAvatarAsync(IFormFileCollection files, string userName);

    Task SavePortfolioProjectAsync(IFormFileCollection files, string projectName, string userLogin);
    
    Task SaveUserInfoAsync(string userLogin, string userInfo);

    Task SaveProfileColorAsync(string userLogin, string color);

    Task<object> SignInAsync(string userName, string password);
}