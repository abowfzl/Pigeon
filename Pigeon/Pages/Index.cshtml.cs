using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pigeon.Factories;
using Pigeon.Models;
using Pigeon.Models.DTOs;
using Pigeon.Services;
using System.Security.Claims;

namespace Pigeon.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly LogRequestService _logRequestService;
    private readonly IndexPageFactory _indexPageFactory;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly TicketService _ticketService; 
    private readonly TicketsFactory _ticketsFactory;


    public IndexModel(ILogger<IndexModel> logger,
        LogRequestService logRequestService,
        IndexPageFactory indexPageFactory,
        UserManager<IdentityUser> userManager,
        TicketService ticketService,
        TicketsFactory ticketsFactory)
    {
        _logger = logger;
        _logRequestService = logRequestService;
        _indexPageFactory = indexPageFactory;
        _userManager = userManager;
        _ticketService = ticketService;
        _ticketsFactory = ticketsFactory;
    }

    public IList<LogRequestModel> LogRequests { get; set; }

    public IList<TicketDTO> Tickets { get; set; }


    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (!User?.Identity?.IsAuthenticated ?? true)
            return Redirect("Identity/Account/Login");

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
            throw new Exception("userId not found");

        if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Admin"))
        {
            var logRequests = await _logRequestService.GetUserLogRequests(userIdClaim, cancellationToken);

            LogRequests = await _indexPageFactory.PrepareLoqRequestsModel(logRequests);
        }
        else if (await _userManager.IsInRoleAsync(await _userManager.GetUserAsync(User), "Specialist"))
        {
            var assignedTicket = await _ticketService.GetAssignedTickets(userIdClaim, cancellationToken);

            Tickets = _ticketsFactory.PrepareTicketDtos(assignedTicket);
        }

        return Page();
    }
}