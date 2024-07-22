using ChatAppAPI.Context;
using ChatAppAPI.ExceptionHandling.Exceptions;
using ChatAppAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Mesajlar.Queries.MesajlasilanKullanicilariGetir
{
    public class MesajlasilanKullanicilariGetirHandler(ChatAppDbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<MesajlasilanKullanicilariGetirRequest, IList<MesajlasilanKullanicilariGetirResponse>>
    {
        public async Task<IList<MesajlasilanKullanicilariGetirResponse>> Handle(MesajlasilanKullanicilariGetirRequest request, CancellationToken cancellationToken)
        {
            var mevcutKullaniciAdi = (httpContextAccessor.HttpContext?.User?.Identity?.Name) ?? throw new Exception("Mevcut Kullanici Bulunamadi.");

            Kullanici? kullanici = await context.Kullanicis
               .Where(k => k.KullaniciAdi == mevcutKullaniciAdi)
               .AsNoTracking()
               .FirstOrDefaultAsync(cancellationToken) ?? throw new NotFoundException("Kullanıcı Bulunamadı");


            var mesajlasilanKullanicilar = await context.Mesajs
                .Where(m => m.GonderenId == kullanici.Id || m.AliciId == kullanici.Id)
                .Select(m => m.GonderenId == kullanici.Id ? m.AliciId : m.GonderenId)
                .Distinct()
                .Select(kullaniciAdi => new
                {
                    MesajlasilanKullanici = context.Kullanicis.Where(u => u.Id == kullaniciAdi).Select(u => new
                    {
                        KullaniciAdi = u.KullaniciAdi,
                        ProfilResmiUrl = u.ProfileImageUrl
                    }).AsNoTracking().FirstOrDefault(),

                    Mesaj = context.Mesajs.Where(msg => (msg.GonderenId == kullanici.Id && msg.AliciId == kullaniciAdi) || (msg.GonderenId == kullaniciAdi && msg.AliciId == kullanici.Id))
                    .OrderByDescending(msg => msg.GonderilmeZamani)
                    .Select(msg => new
                    {
                        SonMesajGonderenAdi = msg.Gonderen.KullaniciAdi,
                        SonGonderilenMesaj = msg.Text,
                        SonGonderilenMesajTarihi = msg.GonderilmeZamani.ToShortDateString(),
                        SonGonderilenMesajSaati = msg.GonderilmeZamani.ToShortTimeString(),

                    })
                    .AsNoTracking()
                    .FirstOrDefault(),
                    GorulmeyenMesajSayisi = context.Mesajs.Count(msg => msg.AliciId == kullanici.Id && msg.GonderenId == kullaniciAdi && !msg.GorulmeDurumu)
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (mesajlasilanKullanicilar.Count == 0) throw new NotFoundException("Mesajlaşılan Kullanıcı Bulunamadı");

            return mesajlasilanKullanicilar
                .Select(m => new MesajlasilanKullanicilariGetirResponse
                {
                    KullaniciAdi = m.MesajlasilanKullanici!.KullaniciAdi,
                    ProfilResmiUrl = m.MesajlasilanKullanici.ProfilResmiUrl,
                    SonMesajGonderenAdi = m.Mesaj!.SonMesajGonderenAdi,
                    SonGonderilenMesaj = m.Mesaj.SonGonderilenMesaj,
                    SonGonderilenMesajTarihi = m.Mesaj.SonGonderilenMesajTarihi,
                    SonGonderilenMesajSaati = m.Mesaj.SonGonderilenMesajSaati,
                    GorulmeyenMesajSayisi = m.GorulmeyenMesajSayisi
                })
                .ToList();

        }
    }
}
