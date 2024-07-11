using FluentValidation;

namespace ChatAppAPI.Servisler.Mesajlar.DTOs
{
    public record MesajGonderDTO(string Text, string AliciAdi);


    public class MesajGonderDTOValidator : AbstractValidator<MesajGonderDTO>
    {
        public MesajGonderDTOValidator()
        {
            RuleFor(model => model.Text)
                .NotEmpty()
                .WithMessage("Mesaj İçeriği Boş Olamaz.");

            RuleFor(model => model.AliciAdi)
                .NotEmpty()
                .WithMessage("Alıcı Adı Boş Olamaz.");

        }
    }
}
