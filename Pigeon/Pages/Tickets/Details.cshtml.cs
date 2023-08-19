using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pigeon.Services;

namespace Pigeon.Pages.Tickets
{
    public class DetailsModel : PageModel
    {
        private readonly TicketService _ticketService;

        public DetailsModel(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            var ticket = await _ticketService.GetTicketById(id, cancellationToken);

            if (ticket is null)
                return NotFound();

            return Page();
        }
    }
}
