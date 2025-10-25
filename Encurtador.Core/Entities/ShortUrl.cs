namespace Encurtador.Core.Entities
{
    public class ShortUrl
    {
        public int Id { get; set; }
        public string LongUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}