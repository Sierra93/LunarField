using System.Data.SqlClient;
using LunarField.Abstractions.User;
using LunarField.Models.User;

namespace LunarField.Services.User;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;

    public UserService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<UserOutput> CreateUserAsync(string userName, string password, string fullName, DateTime dateYear)
    {
        var firstName = fullName.Split(" ")[0];
        var lastName = fullName.Split(" ")[1];
        var secondName = fullName.Split(" ")[2];

        var query = "INSERT INTO dbo.Users" +
                    "(UserLogin, UserPassword, FirstName, LastName, SecondName, DateYear)" +
                    $"VALUES('{userName}', '{password}', '{firstName}', '{lastName}', '{secondName}', '{dateYear}');";

        await using var connection = new SqlConnection(_configuration["MsSqlConnection"]);
        var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        try
        {
            await command.ExecuteNonQueryAsync();
            await connection.CloseAsync();

            var result = new UserOutput
            {
                UserLogin = userName,
                DateYear = dateYear,
                FullName = fullName
            };

            return result;
        }

        catch (Exception e)
        {
            await connection.CloseAsync();
            Console.WriteLine(e);
            throw;
        }

        finally
        {
            await connection.CloseAsync();
        }
    }
}