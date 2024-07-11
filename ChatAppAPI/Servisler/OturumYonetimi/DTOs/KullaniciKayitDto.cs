using FluentValidation;

namespace ChatAppAPI.Servisler.OturumYonetimi.DTOs
{
    public record KullaniciKayitDto(string KullaniciAdi, string KullaniciSifresi, string KullaniciSifresiTekrar);


    public class KullaniciKayitDtoValidator : AbstractValidator<KullaniciKayitDto>
    {
        public KullaniciKayitDtoValidator()
        {
            RuleFor(dto => dto.KullaniciAdi)
                .NotEmpty()
                .WithMessage("Kullanıcı Adı Boş Olamaz.");

            RuleFor(dto => dto.KullaniciSifresi)
                .NotEmpty()
                .WithMessage("Şifre Zorunlu.")
                .MinimumLength(6)
                .WithMessage("Şifre En Az 6 Karakter Uzunluğunda Olmalıdır.");

            RuleFor(dto => dto.KullaniciSifresiTekrar)
                .NotEmpty()
                .WithMessage("Şifre Tekrarı Zorunlu.")
                .Equal(dto => dto.KullaniciSifresi)
                .WithMessage("Girmiş Olduğunuz Parola Birbiri İle Eşleşmiyor.");
        }
    }
}
