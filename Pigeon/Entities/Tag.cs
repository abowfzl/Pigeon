namespace Pigeon.Entities
{
    public class Tag : BaseEntity
    {
        public int UrlId { get; set; }

        public string Title { get; set; }

        public virtual Url Url { get; set; }
    }
}
