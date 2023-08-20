using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pigeon.Factories;
using Pigeon.Models.DTOs;
using Pigeon.Services;

namespace Pigeon.Pages.Tickets;

public class DetailsModel : PageModel
{
    private readonly TicketService _ticketService;
    private readonly TicketsFactory _ticketsFactory;
    private readonly UserManager<IdentityUser> _userManager;

    public DetailsModel(TicketService ticketService,
        TicketsFactory ticketsFactory,
        UserManager<IdentityUser> userManager)
    {
        _ticketService = ticketService;
        _ticketsFactory = ticketsFactory;
        _userManager = userManager;
    }

    [BindProperty]
    public TicketDTO TicketDto { get; set; }

    public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
    {
        var ticket = await _ticketService.GetTicketById(id, cancellationToken);

        if (ticket is null)
            return NotFound();

        TicketDto = _ticketsFactory.PrepareTicketDto(ticket);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await _ticketService.Update(TicketDto, cancellationToken);

        return RedirectToPage("/Index");
    }
}
