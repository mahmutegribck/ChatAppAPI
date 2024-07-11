using FluentValidation;

namespace ChatAppAPI.Servisler.OturumYonetimi.DTOs
{
    public record KullaniciAdiIleGirisYapDTO(string KullaniciAdi);


    public class KullaniciAdiIleGirisYapDTOValidator : AbstractValidator<KullaniciAdiIleGirisYapDTO>
    {
        public KullaniciAdiIleGirisYapDTOValidator()
        {
            RuleFor(dto => dto.KullaniciAdi)
                .NotEmpty()
                .WithMessage("Kullanıcı Adı Zorunlu.");
        }
    }
}
