using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Servisler.OturumYonetimi.DTOs
{
    public class KullaniciGirisDto
    {
        [Required(ErrorMessage = "Isim zorunlu")]
        public required string KullaniciAdi { get; set; }


        [Required(ErrorMessage = "Sifre zorunlu")]
        [DataType(DataType.Password)]
        public required string KullaniciSifresi { get; set; }
    }


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
