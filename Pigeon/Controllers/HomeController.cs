using Microsoft.AspNetCore.Mvc;
using Pigeon.Factories;
using Pigeon.Models;
using Pigeon.Services;
using System.Diagnostics;

namespace Pigeon.Controllers;

public class HomeController : BaseController
{
    private readonly LogRequestService _logRequestService;
    private readonly ILogger<HomeController> _logger;
    private readonly IndexPageFactory _indexPageFactory;
    private readonly TicketService _ticketService;

    public HomeController(ILogger<HomeController> logger,
        LogRequestService logRequestService,
        IndexPageFactory indexPageFactory,
        TicketService ticketService)
    {
        _logger = logger;
        _logRequestService = logRequestService;
        _indexPageFactory = indexPageFactory;
        _ticketService = ticketService;
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