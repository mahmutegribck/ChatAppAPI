using FluentValidation;
using MediatR;

namespace ChatAppAPI.OturumYonetimi.Commands.GirisYap
{
    public class GirisYapRequest : IRequest<GirisYapResponse>
    {
        public required string KullaniciAdi { get; set; }
        public required string KullaniciSifresi { get; set; }

    }

    public class GirisYapRequestValidator : AbstractValidator<GirisYapRequest>
    {
        public GirisYapRequestValidator()
        {
            RuleFor(x => x.KullaniciAdi)
                .NotEmpty().WithMessage("Kullanıcı Adı boş olamaz.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Kullanıcı Adı sadece boşluk karakterlerinden oluşamaz.");

            RuleFor(x => x.KullaniciSifresi)
                .NotEmpty().WithMessage("Kullanıcı Şifresi boş olamaz.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Kullanıcı Şifresi sadece boşluk karakterlerinden oluşamaz.");
        }
    }
}
