using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pigeon.Factories;
using Pigeon.Models;
using Pigeon.Services;
using System.Security.Claims;

namespace Pigeon.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly LogRequestService _logRequestService;
    private readonly IndexPageFactory _indexPageFactory;

    public IndexModel(ILogger<IndexModel> logger,
        LogRequestService logRequestService,
        IndexPageFactory indexPageFactory)
    {
        _logger = logger;
        _logRequestService = logRequestService;
        _indexPageFactory = indexPageFactory;
    }

    public IList<LogRequestModel> LogRequests { get; set; }


    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        if (!User?.Identity?.IsAuthenticated ?? true)
            return Redirect("Identity/Account/Login");

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
            throw new Exception("userId not found");

        var logRequests = await _logRequestService.GetUserLogRequests(userIdClaim, cancellationToken);

        LogRequests = await _indexPageFactory.PrepareLoqRequestsModel(logRequests);

        return Page();
    }
}