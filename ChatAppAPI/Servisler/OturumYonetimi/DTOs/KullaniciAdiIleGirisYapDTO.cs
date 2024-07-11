using FluentValidation;

namespace ChatAppAPI.Servisler.OturumYonetimi.DTOs
{
    public class KullaniciAdiIleGirisYapDTO
    {
        public required string KullaniciAdi { get; set; }
    }


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
