using FluentValidation;

namespace ChatAppAPI.Servisler.OturumYonetimi.DTOs
{
    public record KullaniciGirisDto(string KullaniciAdi, string KullaniciSifresi);


    public class KullaniciGirisDtoValidator : AbstractValidator<KullaniciGirisDto>
    {
        public KullaniciGirisDtoValidator()
        {
            RuleFor(dto => dto.KullaniciAdi)
                .NotEmpty()
                .WithMessage("Kullanıcı Adı Zorunlu.");

            RuleFor(dto => dto.KullaniciSifresi)
                .NotEmpty()
                .WithMessage("Şifre Zorunlu.");

        }
    }
}
