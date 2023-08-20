using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pigeon.Enums;
using Pigeon.Factories;
using Pigeon.Models.DTOs;
using Pigeon.Models;
using Pigeon.Services;

namespace Pigeon.Pages.Tickets;

public class NewModel : PageModel
{
    private readonly IndexPageFactory _indexPageFactory;
    private readonly TicketService _ticketService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly LogRequestService _logRequestService;

    public NewModel(IndexPageFactory indexPageFactory,
        TicketService ticketService,
        UserManager<IdentityUser> userManager,
        LogRequestService logRequestService)
    {
        _indexPageFactory = indexPageFactory;
        _ticketService = ticketService;
        _userManager = userManager;
        _logRequestService = logRequestService;
    }

    [BindProperty]
    public TicketModel TicketModel { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var logRequest = await _logRequestService.GetLogRequestById(id, CancellationToken.None);

        if (logRequest is null)
            return NotFound();

        TicketModel = await _indexPageFactory.PrepareNewTicketModel(id);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var ticketDto = new TicketDTO()
        {
            AdminComment = TicketModel.AdminComment,
            LogRequestId = TicketModel.LogRequestId,
            Result = TicketModel.Result,
            Status = SystemFelowStatus.Pending,
            AssignedTo = TicketModel.AssignedTo,
        };

        await _ticketService.CreateTicket(ticketDto, user.Id, cancellationToken);

        return RedirectToPage("/Index");
    }
}
