using FluentValidation;

namespace ChatAppAPI.Servisler.Mesajlar.DTOs
{
    public class MesajGonderDTO
    {
        public required string Text { get; set; }
        public required string AliciAdi { get; set; }
        public DateTime GonderilmeZamani { get; set; } = DateTime.Now;

    }


    public class MesajGonderDTOValidator : AbstractValidator<MesajGonderDTO>
    {
        public MesajGonderDTOValidator()
        {
            RuleFor(model => model.Text)
                .NotEmpty()
                .WithMessage("Mesaj İçeriği Boş Olamaz.");

            RuleFor(model => model.AliciAdi)
                .NotEmpty()
                .WithMessage("AliciAdi Boş Olamaz.");

            RuleFor(model => model.GonderilmeZamani)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Gönderilme Zamanı Gelecekte Olamaz.");

        }
    }
}
