using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pigeon.Factories;
using Pigeon.Models;
using Pigeon.Models.DTOs;
using Pigeon.Services;

namespace Pigeon.Pages.Tickets;

public class ListModel : PageModel
{
    private readonly TicketService _ticketService;
    private readonly TicketsFactory _ticketsFactory;

    public ListModel(TicketService ticketService, TicketsFactory ticketsFactory)
    {
        _ticketService = ticketService;
        _ticketsFactory = ticketsFactory;
    }

    public IList<TicketDTO> TicketDTOs { get; set; }

    public async Task<IActionResult> OnGet(string userId, CancellationToken cancellationToken)
    {
        var assignedTicket = await _ticketService.GetAssignedTickets(userId, cancellationToken);

        TicketDTOs = _ticketsFactory.PrepareTicketDtos(assignedTicket);

        return Page();
    }
}
