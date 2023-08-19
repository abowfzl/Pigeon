using System.Net;

namespace Pigeon.Entities;

public class LogRequest : BaseEntity
{
    public int FromUrlId { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public bool Reviewed { get; set; }

    public int UrlId { get; set; }

    public virtual Url Url { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; }
}