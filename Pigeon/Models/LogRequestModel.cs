using Pigeon.Models.DTOs;

namespace Pigeon.Models
{
    public class LogRequestModel
    {
        public int Id { get; set; }

        public string? FromUrl { get; set; }

        public string? StatusCodeStr { get; set; }

        public bool Reviewed { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatorName { get; set; }

        public IList<TicketDTO> Tickets { get; set; } = new List<TicketDTO>();
    }
}
