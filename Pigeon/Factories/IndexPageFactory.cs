using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pigeon.Entities;
using Pigeon.Models;
using Pigeon.Services;

namespace Pigeon.Factories;

public class IndexPageFactory
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly TicketsFactory _ticketsFactory;

    public IndexPageFactory(UserManager<IdentityUser> userManager,
        TicketsFactory ticketsFactory)
    {
        _userManager = userManager;
        _ticketsFactory = ticketsFactory;
    }
    public Task<List<LogRequestModel>> PrepareLoqRequestsModel(IList<LogRequest> logRequests)
    {
        var logRequestModel = logRequests.Select(l => new LogRequestModel()
        {
            FromUrl = l.Url?.UrlStr ?? string.Empty,
            Reviewed = l.Reviewed,
            StatusCodeStr = Enum.GetName(l.StatusCode),
            CreatedAt = l.CreatedAt,
            CreatorName = l.Url.User.UserName,
            Id = l.Id,
            HasAnyTicket = l.Tickets.Any()
        }).ToList();

        return Task.FromResult(logRequestModel);
    }

    public Task<LogRequestModel> PrepareLogRequestModel(LogRequest logRequest)
    {
        if (logRequest is null)
            return Task.FromResult(new LogRequestModel());

        var model = new LogRequestModel()
        {
            Id = logRequest.Id,
            FromUrl = logRequest.Url?.UrlStr ?? string.Empty,
            Reviewed = logRequest.Reviewed,
            CreatedAt = logRequest.CreatedAt,
            CreatorName = logRequest?.Url?.User.UserName ?? string.Empty,
            StatusCodeStr = Enum.GetName(logRequest.StatusCode),
            Tickets = _ticketsFactory.PrepareTicketDtos(logRequest.Tickets)
        };

        return Task.FromResult(model);
    }

    public async Task<TicketModel> PrepareNewTicketModel(int id)
    {
        var users = await _userManager.GetUsersInRoleAsync("Specialist");

        var model = new TicketModel()
        {
            LogRequestId = id,
            Users = users.Select(u => new SelectListItem(u.UserName, u.Id)).ToList(),
        };

        return await Task.FromResult(model);
    }
}