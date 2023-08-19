using Pigeon.Enums;

namespace Pigeon.Entities;

public class Ticket : BaseEntity
{
    public int LogRequestId { get; set; }

    public string CreatedBy { get; set; }

    public string AssignedTo { get; set; }

    public SystemFelowStatus Status { get; set; }

    public string? AdminComment { get; set; }

    public string? Result { get; set; }

    public virtual LogRequest LogRequest { get; set; }
}