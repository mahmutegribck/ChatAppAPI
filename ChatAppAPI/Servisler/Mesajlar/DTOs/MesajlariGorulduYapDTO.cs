using FluentValidation;

namespace ChatAppAPI.Servisler.Mesajlar.DTOs
{
    public record MesajlariGorulduYapDTO(List<int> MesajIds);


    public class MesajlariGorulduYapDTOValidator : AbstractValidator<MesajlariGorulduYapDTO>
    {
        public MesajlariGorulduYapDTOValidator()
        {
            RuleFor(model => model.MesajIds)
                .NotEmpty()
                .WithMessage("Mesaj Id'leri Boş Olamaz.")
                .Must(ids => ids.All(id => id > 0))
                .WithMessage("Mesaj Id'leri 0'dan Büyük Olmalıdır.");
        }
    }
}
