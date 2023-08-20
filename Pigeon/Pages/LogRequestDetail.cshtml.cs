using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pigeon.Factories;
using Pigeon.Models;
using Pigeon.Services;

namespace Pigeon.Pages;

public class LogRequestDetailModel : PageModel
{
    private readonly LogRequestService _logRequestService;
    private readonly IndexPageFactory _indexPageFactory;

    public LogRequestDetailModel(LogRequestService logRequestService,
        IndexPageFactory indexPageFactory)
    {
        _logRequestService = logRequestService;
        _indexPageFactory = indexPageFactory;
    }

    public LogRequestModel LogRequest { get; set; }

    public async Task<IActionResult> OnGet(int id)
    {
        var logRequest = await _logRequestService.GetLogRequestById(id, CancellationToken.None);

        if (logRequest is null)
            return NotFound();

        LogRequest = await _indexPageFactory.PrepareLogRequestModel(logRequest!);

        return Page();

    }
}
