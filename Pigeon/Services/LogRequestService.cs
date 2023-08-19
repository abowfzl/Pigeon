using Pigeon.Entities;
using Microsoft.EntityFrameworkCore;
using Pigeon.Data;

namespace Pigeon.Services;

public class LogRequestService
{
    private readonly PigeonDbContext _dbContext;

    public LogRequestService(PigeonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task InsertLogRequests(IList<LogRequest> logRequests, CancellationToken cancellationToken)
    {
        await _dbContext.LogRequests.AddRangeAsync(logRequests, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<LogRequest>> GetUserLogRequests(string userId, CancellationToken cancellationToken)
    {
        var logRequests = await _dbContext.LogRequests
            //.Where(l => l.Reviewed == false)
            .Where(x => x.Url.UserId == userId)
            .OrderByDescending(u => u.Id)
            .Skip(0)
            .Take(10)
            .ToListAsync(cancellationToken);

        return logRequests;
    }

    public async Task<LogRequest?> GetLogRequestById(int logRequestId, CancellationToken cancellationToken)
    {
        var logRequests = await _dbContext.LogRequests
            .Include(l => l.Url)
            .ThenInclude(u => u.User)
            .Where(x => x.Id == logRequestId)
            .OrderByDescending(u => u.Id)
            .FirstOrDefaultAsync(cancellationToken);

        return logRequests;
    }

    public async Task SetAsReviewed(int id, CancellationToken cancellationToken)
    {
        var logRequest = await _dbContext.LogRequests.FindAsync(new object?[] { id }, cancellationToken: cancellationToken) ?? throw new Exception($"log request with id:{id} not found");
       
        logRequest.Reviewed = true;

        _dbContext.LogRequests.Update(logRequest);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}