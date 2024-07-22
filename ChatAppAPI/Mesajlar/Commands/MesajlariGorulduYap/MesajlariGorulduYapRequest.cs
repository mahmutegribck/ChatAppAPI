using FluentValidation;
using MediatR;

namespace ChatAppAPI.Mesajlar.Commands.MesajlariGorulduYap
{
    public class MesajlariGorulduYapRequest : IRequest
    {
        public required List<int> MesajIds { get; set; }
    }


    public class MesajlariGorulduYapRequestValidator : AbstractValidator<MesajlariGorulduYapRequest>
    {
        public MesajlariGorulduYapRequestValidator()
        {
            RuleFor(x => x.MesajIds)
                .NotEmpty().WithMessage("Mesaj Ids boş olamaz.")
                .Must(x => x.All(id => id.GetType() == typeof(int))).WithMessage("Mesaj Ids sadece int değerler içermelidir.")
                .Must(x => x.All(id => id > 0)).WithMessage("Mesaj Ids sıfırdan büyük int değerler içermelidir.");
        }
    }
}
