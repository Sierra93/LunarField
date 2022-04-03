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

    public IActionResult SignIn() => View();

    public IActionResult SignUp() => View();

    [HttpPost]
    [ActionName("CreateUser")]
    public async Task<UserOutput> CreateUserAsync(UserInput userInput)
    {
        var result = await _userService.CreateUserAsync(userInput.UserLogin, userInput.UserPassword, userInput.FullName, userInput.DateYear);

        return result;
    }
    
    [HttpPost]
    [ActionName("SignIn")]
    public async Task<object> SignInAsync(UserInput userInput)
    {
        var result = await _userService.SignInAsync(userInput.UserLogin, userInput.UserPassword);

        return result;
    }

    public async Task<IActionResult> Profile()
    {
        var result = await _userService.GetProfileDataAsync("test");
        
        return View(result);
    }
    
    [HttpGet]
    [ActionName("GetProfileData")]
    public async Task<List<ProfileDataOutput>> GetProfileDataAsync(string userName)
    {
        var result = await _userService.GetProfileDataAsync(userName);
        
        return result;
    }

    [HttpPost]
    [ActionName("SaveUserAvatar")]
    public async Task<IActionResult> SaveUserAvatarAsync(UserInput userInput)
    {
        await _userService.SaveUserAvatarAsync(Request.Form.Files, userInput.UserLogin);

        return RedirectToAction("Profile");
    }
    
    [HttpPost]
    [ActionName("SavePortfolioProject")]
    public async Task<IActionResult> SavePortfolioProjectAsync(UserInput userInput)
    {
        await _userService.SavePortfolioProjectAsync(Request.Form.Files, userInput.ProjectName, userInput.UserLogin);

        return RedirectToAction("Profile");
    }
    
    [HttpPost]
    [ActionName("SaveUserInfo")]
    public async Task<IActionResult> SaveUserInfoAsync(UserInput userInput)
    {
        await _userService.SaveUserInfoAsync(userInput.UserLogin, userInput.UserInfo);

        return RedirectToAction("Profile");
    }
    
    [HttpPost]
    [ActionName("SaveProfileColor")]
    public async Task<IActionResult> SaveProfileColorAsync(UserInput userInput)
    {
        await _userService.SaveProfileColorAsync(userInput.UserLogin, userInput.ProfileColor);

        return RedirectToAction("Profile");
    }
}