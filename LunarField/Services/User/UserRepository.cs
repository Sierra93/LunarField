using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LunarField.Abstractions.User;
using LunarField.Models.User;
using Microsoft.Extensions.Configuration;

namespace LunarField.Services.User
{
    public sealed class UserRepository : IUserRepository
{
    private readonly IConfiguration _configuration;

    public UserRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<UserOutput> CreateUserAsync(string userName, string password, string firstName, string lastName,
        string secondName, DateTime dateYear)
    {
        await using var connection = new SqlConnection(_configuration["MsSqlConnection"]);

        try
        {
            var formatDate = dateYear.ToString().Split(" ").FirstOrDefault();
            var newDate = formatDate.Split(".").Reverse().ToList();
            var date = newDate[0] + "/" + newDate[1] + "/" + newDate[2];
            var query = "INSERT INTO dbo.Users" +
                        "(UserLogin, UserPassword, FirstName, LastName, SecondName, DateYear)" +
                        $"VALUES('{userName}', '{password}', '{firstName}', '{lastName}', '{secondName}', '{date}');";
            var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            
            var queryGetUserId = $"SELECT UserId FROM dbo.Users WHERE UserLogin = '{userName}'";
            var commandGetUserId = new SqlCommand(queryGetUserId, connection);

            // Найдет UserId пользователя.
            var userId = await commandGetUserId.ExecuteScalarAsync();
            
            // Заведет портфолио новому пользователю.
            var queryAddPortfolio = $"INSERT INTO dbo.UserPortfolio (UserId, ProjectName, ProjectPhoto, UserInfo, UserIcon, UserTitle, ProfilePhoto, ProfileColor) VALUES ({userId}, '', '', '', '', '', '', '')";
            var commandAddPortfolio = new SqlCommand(queryAddPortfolio, connection);
            await commandAddPortfolio.ExecuteScalarAsync();

            var result = new UserOutput
            {
                UserLogin = userName,
                DateYear = dateYear,
                FullName = firstName + " " + lastName + " " + secondName
            };

            return result;
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task<List<ProfileDataOutput>> GetProfileDataAsync(string userName)
    {
        await using var connection = new SqlConnection(_configuration["MsSqlConnection"]);

        try
        {
            var result = new List<ProfileDataOutput>();
            var queryGetUserId = $"SELECT UserId FROM dbo.Users WHERE UserLogin = '{userName}'";
            var commandGetUserId = new SqlCommand(queryGetUserId, connection);

            await connection.OpenAsync();

            // Найдет UserId пользователя.
            var userId = await commandGetUserId.ExecuteScalarAsync();

            var queryGetProjectData = $"SELECT * FROM dbo.UserPortfolio WHERE UserId = {userId}";
            var commandGetProjectData = new SqlCommand(queryGetProjectData, connection);

            await using var reader = await commandGetProjectData.ExecuteReaderAsync();

            // Если пришли хоть какие то записи.
            if (reader.HasRows)
            {
                // Читает строки.
                while (await reader.ReadAsync())
                {
                    result.Add(new ProfileDataOutput
                    {
                        UserTitle = reader["UserTitle"].ToString(),
                        ProfileUserInfo = reader["UserInfo"].ToString(),
                        ProjectTitle = reader["ProjectName"].ToString(),
                        ProjectUrl = reader["ProjectPhoto"].ToString(),
                        UserIcon = reader["UserIcon"].ToString(),
                        ProfileColor = reader["ProfileColor"].ToString()
                    });
                }
            }

            return result;
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        finally
        {
            await connection.CloseAsync();
        }
    }

    public async Task SaveUserAvatarAsync(string userName, string fileName)
    {
        await using var connection = new SqlConnection(_configuration["MsSqlConnection"]);

        try
        {
            var queryGetUserId = $"SELECT UserId FROM dbo.Users WHERE UserLogin = '{userName}'";
            var commandGetUserId = new SqlCommand(queryGetUserId, connection);

            await connection.OpenAsync();

            // Найдет UserId пользователя.
            var userId = await commandGetUserId.ExecuteScalarAsync();

            // Обновит изображение аватарки пользователя.
            var query = "UPDATE dbo.UserPortfolio SET UserIcon = " + "\'" + fileName + "\'" + " WHERE UserId = " +
                        userId;
            var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SavePortfolioProjectAsync(string userName, string fileName)
    {
        await using var connection = new SqlConnection(_configuration["MsSqlConnection"]);

        try
        {
            var queryGetUserId = $"SELECT UserId FROM dbo.Users WHERE UserLogin = '{userName}'";
            var commandGetUserId = new SqlCommand(queryGetUserId, connection);

            await connection.OpenAsync();

            // Найдет UserId пользователя.
            var userId = await commandGetUserId.ExecuteScalarAsync();

            // Обновит изображение проекта пользователя.
            var query = "UPDATE dbo.UserPortfolio SET ProjectPhoto = " + "\'" + fileName + "\'" + " WHERE UserId = " +
                        userId;
            var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SaveUserInfoAsync(string userLogin, string userInfo)
    {
        await using var connection = new SqlConnection(_configuration["MsSqlConnection"]);
        
        try
        {
            var queryGetUserId = $"SELECT UserId FROM dbo.Users WHERE UserLogin = '{userLogin}'";
            var commandGetUserId = new SqlCommand(queryGetUserId, connection);

            await connection.OpenAsync();

            // Найдет UserId пользователя.
            var userId = await commandGetUserId.ExecuteScalarAsync();
            
            // Обновит информацию о пользователе.
            var query = "UPDATE dbo.UserPortfolio SET UserInfo = " + "\'" + userInfo + "\'" + " WHERE UserId = " + userId;
            var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }

        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task SaveProfileColorAsync(string userLogin, string color)
    {
        await using var connection = new SqlConnection(_configuration["MsSqlConnection"]);
        
        try
        {
            var queryGetUserId = $"SELECT UserId FROM dbo.Users WHERE UserLogin = '{userLogin}'";
            var commandGetUserId = new SqlCommand(queryGetUserId, connection);

            await connection.OpenAsync();

            // Найдет UserId пользователя.
            var userId = await commandGetUserId.ExecuteScalarAsync();
            
            // Обновит цвет фона профиля.
            var query = "UPDATE dbo.UserPortfolio SET ProfileColor = " + "\'" + color + "\'" + " WHERE UserId = " + userId;
            var command = new SqlCommand(query, connection);
            await command.ExecuteNonQueryAsync();
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> SignInAsync(string userName, string password)
    {
        await using var connection = new SqlConnection(_configuration["MsSqlConnection"]);
        
        try
        {
            await connection.OpenAsync();
            var query = $"SELECT UserLogin FROM dbo.Users WHERE UserLogin = '{userName}' AND UserPassword = '{password}'";
            var command = new SqlCommand(query, connection);
            var result = await command.ExecuteScalarAsync();

            return new {UserLogin = (string)result};
        }
        
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
}

