using Encurtador.Core.Entities;
using Encurtador.Core.Interfaces;
using Encurtador.WebApi.Models;
using Encurtador.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Encurtador.WebApi.Controllers
{
    [ApiController]
    [Route("/")] // rota base

    public class UrlController : ControllerBase
    {
        private readonly IUrlRepository _repository;
        private readonly UrlShorteningService _service;

        public UrlController(IUrlRepository repository, UrlShorteningService service)
        {
            _repository = repository;
            _service = service;
        }

        [HttpPost("api/shorten")]
        public async Task<IActionResult> Shorten([FromBody] ShortenRequestDto request)
        {
            var shortCode = await _service.GenerateUniqueShortCode();
            var shortUrl = new ShortUrl
            {
                LongUrl = request.Url,
                ShortCode = shortCode,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(shortUrl);

            var resultUrl = $"{Request.Scheme}://{Request.Host}/{shortCode}";
            return Ok(new { ShortUrl = resultUrl });
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToUrl(string shortCode)
        {
            var url = await _repository.GetByShortCodeAsync(shortCode);

            if (url == null)
            {
                return NotFound();
            }

            return Redirect(url.LongUrl);
        }

    }
}