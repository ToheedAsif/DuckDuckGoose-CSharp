using DuckDuckGoose.Models;
using DuckDuckGoose.Models.Requests;
using DuckDuckGoose.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DuckDuckGoose.Repositories;

namespace DuckDuckGoose.Controllers;

[Route("honks")]
public class HonkController : Controller
{
    private readonly ILogger<HonkController> _logger;
    private readonly IHonkRepo _honks;

    public HonkController
    (
        ILogger<HonkController> logger,
        IHonkRepo honks
    )
    {
        _logger = logger;
        _honks = honks;
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
