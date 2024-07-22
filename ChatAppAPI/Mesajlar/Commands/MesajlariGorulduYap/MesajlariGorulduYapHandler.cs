using ChatAppAPI.Context;
using ChatAppAPI.ExceptionHandling.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Mesajlar.Commands.MesajlariGorulduYap
{
    public class MesajlariGorulduYapHandler(ChatAppDbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<MesajlariGorulduYapRequest>
    {
        public async Task Handle(MesajlariGorulduYapRequest request, CancellationToken cancellationToken)
        {
            var mevcutKullaniciAdi = (httpContextAccessor.HttpContext?.User?.Identity?.Name) ?? throw new Exception("Mevcut Kullanici Bulunamadi.");

            var alici = await context.Kullanicis
                .Where(k => k.KullaniciAdi == mevcutKullaniciAdi)
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            var mesajlar = await context.Mesajs
                                .Where(m => request.MesajIds.Contains(m.Id) && m.AliciId == alici.Id)
                                .ToListAsync(cancellationToken);

            if (mesajlar.Count == 0) throw new NotFoundException("Okunmamış Mesaj Bulunamadı.");

            foreach (var mesaj in mesajlar)
            {
                mesaj.GorulmeDurumu = true;
            }

            context.Mesajs.UpdateRange(mesajlar);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
