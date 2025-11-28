using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Utulek1.Application.Abstraction;
using Utulek1.Models;
using Utulek1.Application.ViewModels;


namespace Utulek1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    IHomeService _homeService;

    public HomeController(ILogger<HomeController> logger,
                          IHomeService homeService)
    {
        _logger = logger;
        _homeService = homeService;
    }

    public IActionResult Index()
    {
        CarouselAnimalViewModel viewModel = _homeService.GetIndexViewModel();
        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

