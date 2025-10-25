using Encurtador.Core.Interfaces;

namespace Encurtador.WebApi.Services
{
    public class UrlShorteningService
    {
        private readonly IUrlRepository _repository;
        private readonly Random _random = new();
        private const int CodeLenth = 6;
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public UrlShorteningService(IUrlRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GenerateUniqueShortCode()
        {
            string code;
            do
            {
                code = new string(Enumerable.Repeat(Chars, CodeLenth)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            }
            while (await _repository.DoesShortCodeExistAsync(code));

            return code;
        }
    }
}