using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using Pigeon.Data;
using Pigeon.Entities;
using Pigeon.Enums;
using Pigeon.Models.DTOs;

namespace Pigeon.Services;

public class TicketService
{
    private readonly PigeonDbContext _dbContext;
    private readonly LogRequestService _logRequestService;

    public TicketService(PigeonDbContext dbContext,
        LogRequestService logRequestService)
    {
        _dbContext = dbContext;
        _logRequestService = logRequestService;
    }

    public async Task<Ticket?> GetTicketById(int id, CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == id);

        return ticket;
    }

    public async Task CreateTicket(TicketDTO ticketDto, string creatorUserId, CancellationToken cancellationToken)
    {
        if (creatorUserId == default)
            throw new ArgumentException($"value of{creatorUserId} must be greater than 0");

        if (ticketDto is null)
            throw new ArgumentNullException(nameof(ticketDto));

        var ticket = new Ticket()
        {
            AdminComment = ticketDto.AdminComment,
            AssignedTo = ticketDto.AssignedTo,
            LogRequestId = ticketDto.LogRequestId,
            Status = SystemFelowStatus.Pending,
            Result = null,
            CreatedBy = creatorUserId,
            CreatedAt = DateTime.Now
        };

        await _dbContext.Tickets.AddAsync(ticket, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        await _logRequestService.SetAsReviewed(ticketDto.LogRequestId, cancellationToken);
    }

    public async Task<IList<Ticket>?> GetAssignedTickets(string userId, CancellationToken cancellationToken)
    {
        var tickets = await _dbContext.Tickets.Where(s => s.AssignedTo == userId).ToListAsync(cancellationToken);

        return tickets;
    }

    public async Task Update(TicketDTO ticketDto, CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets.FindAsync(new object?[] { ticketDto.Id }, cancellationToken: cancellationToken);

        ticket.AdminComment = ticketDto.AdminComment;
        ticket.LogRequestId = ticketDto.LogRequestId;
        ticket.Result = ticketDto.Result;
        ticket.Status = ticketDto.Status;
        ticket.AssignedTo = ticketDto.AssignedTo;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}