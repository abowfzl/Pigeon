using NuGet.Packaging;
using Pigeon.Contracts;
using Pigeon.Entities;
using Pigeon.Services;

namespace Pigeon.Crawler;

public class LinkCrawler : ICrawler
{
    private readonly UrlService _urlService;
    private readonly LogRequestService _logRequestService;
    private readonly FetchService _fetchService;
    private readonly HttpClient _httpClient;
    private readonly TagService _tagService;

    public LinkCrawler(UrlService urlService,
        FetchService linkFetchService,
        HttpClient httpClient,
        LogRequestService logRequestService,
        TagService tagService)
    {
        _urlService = urlService;
        _fetchService = linkFetchService;
        _httpClient = httpClient;
        _logRequestService = logRequestService;
        _tagService = tagService;
    }

    public async Task Crawl(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(5000, cancellationToken);

            var urls = await _urlService.GetActiveUrlsAsync(cancellationToken);

            var newUrls = new List<Url>();

            var newTags = new List<Tag>();

            var logRequests = new List<LogRequest>();

            foreach (var url in urls)
            {
                await Task.Delay(Random.Shared.Next(300, 900), cancellationToken);

                var respose = await _httpClient.GetAsync(url.UrlStr, cancellationToken);

                if (respose.IsSuccessStatusCode)
                {
                    var content = await respose.Content.ReadAsStringAsync(cancellationToken);

                    var uri = new Uri(url.UrlStr);

                    var links = await _fetchService.FetchLinksAsync(uri.Scheme + "://" + uri.Host, content, cancellationToken);

                    var tags = await _fetchService.FetchTagsAsync(content, cancellationToken);

                    newTags.AddRange(tags.Select(t => new Tag()
                    {
                        CreatedAt = DateTime.Now,
                        Title = t.Title,
                        UrlId = url.Id
                    }));

                    newUrls = newUrls.Where(u => !urls.Select(s=>s.UrlStr).Contains(u.UrlStr)).DistinctBy(u => u.UrlStr).ToList();

                    newUrls.AddRange(links.Select(l => new Url()
                    {
                        UrlStr = l.Url,
                        ParentId = url.Id,
                        IsNew = true,
                        CreatedAt = DateTime.Now,
                        UserId = url.UserId
                    }));
                }
                logRequests.Add(new LogRequest()
                {
                    CreatedAt = DateTime.Now,
                    FromUrlId = url.Id,
                    StatusCode = respose.StatusCode,
                    Reviewed = false,
                    UrlId = url.Id,
                });
                url.IsNew = false;
            }

            await _logRequestService.InsertLogRequests(logRequests, cancellationToken);

            await _tagService.InsertTags(newTags, cancellationToken);

            await _urlService.InsertUrls(newUrls, cancellationToken);

            await _urlService.UpdateUrls(urls, cancellationToken);
        }
    }
}