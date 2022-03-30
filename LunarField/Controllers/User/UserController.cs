using LunarField.Abstractions.User;
using LunarField.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LunarField.Controllers.User;

public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult SignIn()
    {
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserInput userInput)
    {
        var result = await _userService.CreateUserAsync(userInput.UserLogin, userInput.UserPassword, userInput.FullName, userInput.DateYear);

        return new OkObjectResult(result);
    }
}