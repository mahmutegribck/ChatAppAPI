using ChatAppAPI.OturumYonetimi.Commands.KayitOl;
using FluentValidation;
using MediatR;

namespace ChatAppAPI.OturumYonetimi.Commands.KullaniciAdiIleGirisYap
{
    public class KullaniciAdiIleGirisYapRequest : IRequest<KullaniciAdiIleGirisYapResponse>
    {
        public required string KullaniciAdi { get; set; }
    }


    public class KullaniciAdiIleGirisYapRequestValidator : AbstractValidator<KullaniciAdiIleGirisYapRequest>
    {
        public KullaniciAdiIleGirisYapRequestValidator()
        {
            RuleFor(x => x.KullaniciAdi)
                .NotEmpty().WithMessage("Kullanıcı Adı boş olamaz.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Kullanıcı Adı sadece boşluk karakterlerinden oluşamaz.");

        }
    }
}
