using HtmlAgilityPack;
using Pigeon.Models.Fetch;
using Pigeon.Models.FetchLink;

namespace Pigeon.Services;

public class FetchService
{
    public async Task<IList<FetchLink>> FetchLinksAsync(string baseAddress, string content, CancellationToken cancellationToken)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(content);
        var fetchedLinks = doc.DocumentNode.SelectNodes("//a[@href]")
            .Select(s => s.Attributes["href"])
            .Where(s => s.Value.Contains('a'))
            .DistinctBy(s => s.Value)
            .Select(s => new FetchLink()
            {
                Url = s.Value
            })
            .Where(s => !s.Url.Contains("ads"))
            .ToList();

        fetchedLinks.ForEach(s =>
        {
            if (!s.Url.StartsWith("www.") && !s.Url.StartsWith("http"))
                s.Url = baseAddress.TrimEnd('/') + s.Url;

        });

        return await Task.FromResult(fetchedLinks);
    }

    public async Task<IList<FetchTag>> FetchTagsAsync(string content, CancellationToken cancellationToken)
    {
        HtmlDocument doc = new();
        doc.LoadHtml(content);
        var tagsDiv = doc.DocumentNode.SelectNodes("//div[contains(@class, 'tags_title dir-rtl')]");

        if (tagsDiv == null)
            return await Task.FromResult(new List<FetchTag>());

        var fetchedTags = tagsDiv.Select(s => s.ChildNodes["a"])
            .DistinctBy(s => s.InnerText)
            .Select(s => new FetchTag()
            {
                Title = s.InnerText
            })
            .ToList();

        return await Task.FromResult(fetchedTags);
    }
}