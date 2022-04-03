using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LunarField.Models.User;

namespace LunarField.Abstractions.User
{
    public interface IUserRepository
    {
        Task<UserOutput> CreateUserAsync(string userName, string password, string firstName, string lastName, string secondName, DateTime dateYear);
    
        Task<List<ProfileDataOutput>> GetProfileDataAsync(string userName);

        Task SaveUserAvatarAsync(string userName, string fileName);

        Task SavePortfolioProjectAsync(string userName, string fileName);

        Task SaveUserInfoAsync(string userLogin, string userInfo);
    
        Task SaveProfileColorAsync(string userLogin, string color);
    
        Task<object> SignInAsync(string userName, string password);
    }
}

