using DuckDuckGoose.Models;
using DuckDuckGoose.Models.Requests;
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

    public IActionResult Index(
        [FromQuery] GetUsersRequest request
    )
    {
        ViewData["IsAuthenticated"] = false;
        var users = _users.GetUsers(request);
        UsersViewModel viewModel = new UsersViewModel
        {
            Users = new Pagination<UserViewModel>
            {
                Page = users.Page,
                PerPage = users.PerPage,
                Items = users.Items.Select(user => new UserViewModel(user, 1, 5)),
                Total = users.Total,
            },
            Filter = request.Filter,
            Search = request.Search,
        };
        return View(viewModel);
    }
}
