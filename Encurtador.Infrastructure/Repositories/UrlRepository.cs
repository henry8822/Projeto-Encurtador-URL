using System.Runtime.CompilerServices;
using Encurtador.Core.Entities;
using Encurtador.Core.Interfaces;
using Encurtador.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Encurtador.Infrastructure.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ApiDbContext _context;

        public UrlRepository(ApiDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ShortUrl shortUrl)
        {
            await _context.ShortUrls.AddAsync(shortUrl);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesShortCodeExistAsync(string shortCode)
        {
            return await _context.ShortUrls.AnyAsync(u => u.ShortCode == shortCode);

        }

        public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode)
        {
            return await _context.ShortUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        }
    }
}