using DuckDuckGoose.Models.ViewModels;
using DuckDuckGoose.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DuckDuckGoose.Controllers;

[Route("users")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserRepo _users;

    public UserController
    (
        ILogger<UserController> logger,
        IUserRepo users
    )
    {
        _logger = logger;
        _users = users;
    }

    [HttpGet("{userId}")]
    public IActionResult UserPage([FromRoute] string userId, [FromQuery] int? page)
    {
        UserViewModel user;
        try
        {
            user = new UserViewModel(_users.GetUserById(userId), page.HasValue ? page.Value : 1, 5);
        }
        catch (ArgumentOutOfRangeException)
        {
            return NotFound();
        }
        return View(user);
    }
}
