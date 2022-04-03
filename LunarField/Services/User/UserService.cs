using System.Data.SqlClient;
using LunarField.Abstractions.User;
using LunarField.Models.User;

namespace LunarField.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<UserOutput> CreateUserAsync(string userName, string password, string fullName, DateTime dateYear)
    {
        try
        {
            var firstName = fullName.Split(" ")[0];
            var lastName = fullName.Split(" ")[1];
            var secondName = fullName.Split(" ")[2];

            var result = await _userRepository.CreateUserAsync(userName, password, firstName, lastName, secondName, dateYear);

            return result;
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<ProfileDataOutput>> GetProfileDataAsync(string userName)
    {
        try
        {
            var result = await _userRepository.GetProfileDataAsync(userName);

            return result;
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SaveUserAvatarAsync(IFormFileCollection files, string userName)
    {
        try
        {
            var folderPath = "wwwroot/images/";
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderPath, files[0].FileName);
            // var path = Path.Combine(
            //     Directory.GetCurrentDirectory(), storePath,
            //     form.Files[0].FileName);
            await using var stream = new FileStream(path, FileMode.Create);
            await files[0].CopyToAsync(stream);
            
            // Обновит аватарку.
            await _userRepository.SaveUserAvatarAsync(userName, "~/images/" + files[0].FileName);
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SavePortfolioProjectAsync(IFormFileCollection files, string projectName, string userLogin)
    {
        try
        {
            var folderPath = "wwwroot/images/";
            var path = Path.Combine(Directory.GetCurrentDirectory(), folderPath, files[0].FileName);
            
            await using var stream = new FileStream(path, FileMode.Create);
            await files[0].CopyToAsync(stream);
            
            // Обновит проект.
            await _userRepository.SavePortfolioProjectAsync(userLogin, "~/images/" + files[0].FileName);
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SaveUserInfoAsync(string userLogin, string userInfo)
    {
        try
        {
            // Обновит информацию о пользователе.
            await _userRepository.SaveUserInfoAsync(userLogin, userInfo);
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SaveProfileColorAsync(string userLogin, string color)
    {
        try
        {
            await _userRepository.SaveProfileColorAsync(userLogin, color);
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> SignInAsync(string userName, string password)
    {
        try
        {
            var result = await _userRepository.SignInAsync(userName, password);

            return result;
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}