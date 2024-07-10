using FluentValidation;

namespace ChatAppAPI.Servisler.Mesajlar.DTOs
{
    public class MesajlariGorulduYapDTO
    {
        public required List<int> MesajIds { get; set; }
    }


    public class MesajlariGorulduYapDTOValidator : AbstractValidator<MesajlariGorulduYapDTO>
    {
        public MesajlariGorulduYapDTOValidator()
        {
            RuleFor(model => model.MesajIds)
                .NotEmpty()
                .WithMessage("Mesaj Id'leri boş olamaz.")
                .Must(ids => ids.All(id => id > 0))
                .WithMessage("Mesaj Id'leri pozitif olmalıdır.");
        }
    }
}
