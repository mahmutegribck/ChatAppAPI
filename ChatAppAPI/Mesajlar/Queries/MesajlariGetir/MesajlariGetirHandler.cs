using ChatAppAPI.Context;
using ChatAppAPI.ExceptionHandling.Exceptions;
using ChatAppAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Mesajlar.Queries.MesajlariGetir
{
    public class MesajlariGetirHandler(ChatAppDbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<MesajlariGetirRequest, IList<MesajlariGetirResponse>>
    {
        public async Task<IList<MesajlariGetirResponse>> Handle(MesajlariGetirRequest request, CancellationToken cancellationToken)
        {
            var mevcutKullaniciAdi = (httpContextAccessor.HttpContext?.User?.Identity?.Name) ?? throw new Exception("Mevcut Kullanici Bulunamadi.");

            if (!await context.Kullanicis.AnyAsync(k => k.KullaniciAdi == request.AliciAdi, cancellationToken)) throw new NotFoundException("Alıcı Kullanıcı Bulunamadı");

            IEnumerable<Mesaj> mesajlar = await context.Mesajs
                .Include(m => m.Gonderen)
                .Include(m => m.Alici)
                .Where(m =>
                    m.Gonderen.KullaniciAdi == mevcutKullaniciAdi &&
                    m.Alici.KullaniciAdi == request.AliciAdi ||
                    m.Gonderen.KullaniciAdi == request.AliciAdi &&
                    m.Alici.KullaniciAdi == mevcutKullaniciAdi)
                .OrderBy(m => m.GonderilmeZamani).Skip((request.SayfaNumarasi - 1) * request.SayfaBuyuklugu).Take(request.SayfaBuyuklugu)
                .ToListAsync(cancellationToken);

            if (!mesajlar.Any()) throw new NotFoundException("Mesaj Bulunamadı");

            var response = mesajlar.Select(m => new MesajlariGetirResponse
            {
                GondericiAdi = m.Gonderen.KullaniciAdi,
                AliciAdi = m.Alici.KullaniciAdi,
                Text = m.Text,
                GonderilmeTarihi = m.GonderilmeZamani.ToShortDateString(),
                GonderilmeSaati = m.GonderilmeZamani.ToShortTimeString()
            }).ToList();

            return response;
        }
    }
}