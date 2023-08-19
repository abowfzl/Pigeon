using Pigeon.Entities;
using Pigeon.Models.DTOs;

namespace Pigeon.Factories
{
    public class TicketsFactory
    {
        public IList<TicketDTO> PrepareTicketDtos(IList<Ticket> tickets)
        {
            return tickets
                 .Select(t => new TicketDTO()
                 {
                     Id = t.Id,
                     LogRequestId = t.LogRequestId,
                     AdminComment = t.AdminComment,
                     AssignedTo = t.AssignedTo,
                     Result = t.Result,
                     Status = t.Status
                 })
                .ToList();
        }
    }
}
