using Microsoft.AspNetCore.Identity;

namespace Pigeon.Entities;

public class Url : BaseEntity
{
    public string? UrlStr { get; set; }

    public int ParentId { get; set; }

    public bool IsNew { get; set; }

    public string UserId { get; set; }

    public virtual IdentityUser User { get; set; }
}