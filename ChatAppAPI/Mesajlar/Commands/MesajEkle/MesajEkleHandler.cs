using ChatAppAPI.Context;
using ChatAppAPI.ExceptionHandling.Exceptions;
using ChatAppAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Mesajlar.Commands.MesajEkle
{
    public class MesajEkleHandler(ChatAppDbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<MesajEkleRequest>
    {
        public async Task Handle(MesajEkleRequest request, CancellationToken cancellationToken)
        {
            var mevcutKullaniciAdi = (httpContextAccessor.HttpContext?.User?.Identity?.Name) ?? throw new Exception("Mevcut Kullanici Bulunamadi.");

            Kullanici? alici = await context.Kullanicis
                .Where(k => k.KullaniciAdi == request.AliciAdi)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Alıcı Bulunamadı");

            Kullanici? gönderici = await context.Kullanicis
                .Where(k => k.KullaniciAdi == mevcutKullaniciAdi)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Gönderici Bulunamadı.");

            Mesaj mesaj = new()
            {
                Text = request.Text,
                GonderilmeZamani = DateTime.Now,
                GonderenId = gönderici.Id,
                AliciId = alici.Id
            };

            await context.Mesajs.AddAsync(mesaj, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
