using DuckDuckGoose.Models;
using DuckDuckGoose.Models.Requests;
using DuckDuckGoose.Models.ViewModels;
using DuckDuckGoose.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using DuckDuckGoose.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DuckDuckGoose.Controllers;

[Route("honks")]
public class HonkController : Controller
{
    private readonly ILogger<HonkController> _logger;
    private readonly IHonkRepo _honks;
    private readonly UserManager<DuckDuckGooseUser> _userManager;
    private readonly IHttpContextAccessor _httpContext;

    public HonkController
    (
        ILogger<HonkController> logger,
        IHonkRepo honks,
        UserManager<DuckDuckGooseUser> userManager,
        IHttpContextAccessor httpContext
    )
    {
        _logger = logger;
        _honks = honks;
        _userManager = userManager;
        _httpContext = httpContext;
    }

    [HttpGet("new")]
    public IActionResult CreateHonkForm()
    {
        return View();
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateHonk([FromForm] CreateHonkRequest newHonkRequest)
    {
        var user = await _userManager.GetUserAsync(_httpContext.HttpContext?.User);
        string userId = user.Id;

        _honks.CreateHonk(newHonkRequest.Content, userId);

        return RedirectToAction("Index");
    }
    
    
    
    public IActionResult Index(
        [FromQuery] GetHonksRequest request
    )
    {
        var honks = _honks.GetHonks(request);
        HonksViewModel viewModel = new HonksViewModel
        {
            Honks = new Pagination<HonkViewModel>
            {
                Page = honks.Page,
                PerPage = honks.PerPage,
                Items = honks.Items.Select(honk => new HonkViewModel(honk)),
                Total = honks.Total,
            },
            Filter = request.Filter,
            Search = request.Search,
        };
        return View(viewModel);
    }
}
