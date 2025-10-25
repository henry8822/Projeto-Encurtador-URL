using Encurtador.WebApi.Models;
using FluentValidation;

namespace Encurtador.WebApi.Validators
{
    
    public class ShortenRequestValidator : AbstractValidator<ShortenRequestDto>
    {
        public ShortenRequestValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty().WithMessage("A URL não pode estar vazia.")
                .Must(BeAValidURL).WithMessage("A URL fornecida não é válida.");
        }

        public bool BeAValidURL(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}