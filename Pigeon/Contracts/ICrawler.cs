namespace Pigeon.Contracts;

public interface ICrawler
{
    Task Crawl(CancellationToken cancellationToken);
}