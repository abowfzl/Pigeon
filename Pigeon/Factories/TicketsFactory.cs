using Microsoft.AspNetCore.Mvc.Rendering;
using Pigeon.Entities;
using Pigeon.Enums;
using Pigeon.Models.DTOs;

namespace Pigeon.Factories;

public class TicketsFactory
{
    private readonly IHtmlHelper _htmlHelper;

    public TicketsFactory(IHtmlHelper htmlHelper)
    {
        _htmlHelper = htmlHelper;
    }

    public IList<TicketDTO> PrepareTicketDtos(IEnumerable<Ticket> tickets)
    {
        return tickets
             .Select(t => new TicketDTO()
             {
                 Id = t.Id,
                 LogRequestId = t.LogRequestId,
                 AdminComment = t.AdminComment,
                 AssignedTo = t.AssignedTo,
                 Result = t.Result,
                 Status = t.Status,
                 AvalibleStatus = _htmlHelper.GetEnumSelectList<SystemFelowStatus>().ToList(),
                 CreatedAt = t.CreatedAt
             })
            .ToList();
    }
    public TicketDTO PrepareTicketDto(Ticket ticket)
    {
        return new TicketDTO()
        {
            Id = ticket.Id,
            LogRequestId = ticket.LogRequestId,
            AdminComment = ticket.AdminComment,
            AssignedTo = ticket.AssignedTo,
            Result = ticket.Result,
            Status = ticket.Status,
            AvalibleStatus = _htmlHelper.GetEnumSelectList<SystemFelowStatus>().ToList(),
            CreatedAt = ticket.CreatedAt
        };
    }
}
