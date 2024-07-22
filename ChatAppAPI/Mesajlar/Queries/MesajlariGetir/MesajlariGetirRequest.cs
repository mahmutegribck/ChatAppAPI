using FluentValidation;
using MediatR;

namespace ChatAppAPI.Mesajlar.Queries.MesajlariGetir
{
    public class MesajlariGetirRequest : IRequest<IList<MesajlariGetirResponse>>
    {
        public required string AliciAdi { get; set; }
        public int SayfaBuyuklugu { get; set; }
        public int SayfaNumarasi { get; set; }
    }

    public class MesajlariGetirRequestValidator : AbstractValidator<MesajlariGetirRequest>
    {
        public MesajlariGetirRequestValidator()
        {
            RuleFor(x => x.AliciAdi)
                .NotEmpty().WithMessage("Alıcı Adı boş olamaz.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Alıcı Adı sadece boşluk karakterlerinden oluşamaz.");

            RuleFor(x => x.SayfaBuyuklugu)
                .NotEmpty().WithMessage("Sayfa büuüklüğü boş olamaz.")
                .GreaterThan(0).WithMessage("Sayfa büyüklüğü sıfırdan büyük olmalıdır.");

            RuleFor(x => x.SayfaNumarasi)
                .NotEmpty().WithMessage("Sayfa numarası boş olamaz.")
                .GreaterThan(0).WithMessage("Sayfa numarası sıfırdan büyük olmalıdır.");
        }
    }
}
