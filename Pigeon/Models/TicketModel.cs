using Microsoft.AspNetCore.Mvc.Rendering;

namespace Pigeon.Models;

public class TicketModel
{
    public TicketModel()
    {
        Users = new List<SelectListItem>();
    }

    public int Id { get; set; }

    public int LogRequestId { get; set; }

    public string AssignedTo { get; set; }

    public string? AdminComment { get; set; }

    public string? Result { get; set; }

    public List<SelectListItem> Users { get; set; }
}
