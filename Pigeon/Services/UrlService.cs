using Microsoft.EntityFrameworkCore;
using Pigeon.Data;
using Pigeon.Entities;

namespace Pigeon.Services;

public class UrlService
{
    private readonly PigeonDbContext _pigeonDbContext;

    public UrlService(PigeonDbContext pigeonDbContext)
    {
        _pigeonDbContext = pigeonDbContext;
    }

    public async Task<IList<Url>> GetActiveUrlsAsync(CancellationToken cancellationToken)
    {
        var urls = await _pigeonDbContext.Urls.Where(u => u.IsNew).ToListAsync(cancellationToken);

        return urls;
    }

    public async Task DeleteUrl(Url url, CancellationToken cancellationToken)
    {
        _pigeonDbContext.Urls.Remove(url);

        await _pigeonDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task InsertUrls(IList<Url> urls, CancellationToken cancellationToken)
    {
        var existedUrls = await _pigeonDbContext.Urls.Select(s => s.UrlStr).ToListAsync(cancellationToken);

        urls = urls.Where(u => !existedUrls.Contains(u.UrlStr)).DistinctBy(u => u.UrlStr).ToList();

        await _pigeonDbContext.AddRangeAsync(urls, cancellationToken);

        await _pigeonDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUrls(IList<Url> urls, CancellationToken cancellationToken)
    {
        _pigeonDbContext.UpdateRange(urls);

        await _pigeonDbContext.SaveChangesAsync(cancellationToken);
    }
}