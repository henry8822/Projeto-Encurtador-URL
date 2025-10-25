using Encurtador.Core.Entities;

namespace Encurtador.Core.Interfaces
{
    public interface IUrlRepository
    {
        Task<ShortUrl?> GetByShortCodeAsync(String ShortCode);
        Task<bool> DoesShortCodeExistAsync(string ShortCode);
        Task AddAsync(ShortUrl shortUrl);
    }   
}