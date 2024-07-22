using FluentValidation;
using MediatR;

namespace ChatAppAPI.Mesajlar.Commands.MesajEkle
{
    public class MesajEkleRequest : IRequest
    {
        public required string Text { get; set; }
        public required string AliciAdi { get; set; }
    }


    public class MesajEkleRequestValidator : AbstractValidator<MesajEkleRequest>
    {
        public MesajEkleRequestValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text boş olamaz.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Text sadece boşluk karakterlerinden oluşamaz.");

            RuleFor(x => x.AliciAdi)
                .NotEmpty().WithMessage("Alıcı Adı boş olamaz.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Alıcı Adı sadece boşluk karakterlerinden oluşamaz.");

        }
    }
}
