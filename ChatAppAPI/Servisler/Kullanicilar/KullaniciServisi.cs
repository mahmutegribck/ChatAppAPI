using AutoMapper;
using ChatAppAPI.Context;
using ChatAppAPI.Models;
using ChatAppAPI.Servisler.Kullanicilar.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Servisler.Kullanicilar
{
    public class KullaniciServisi(ChatAppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor) : IKullaniciServisi
    {
        public string? MevcutKullaniciAdi => httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public KullaniciGetirDTO KullaniciGetir(string kullaniciAdi)
        {
            return mapper.Map<KullaniciGetirDTO>(context.Kullanicis.Where(k => k.KullaniciAdi == kullaniciAdi).AsNoTracking().FirstOrDefault());
        }

        public async Task<KullaniciGetirDTO> MevcutKullaniciGetir(CancellationToken cancellationToken)
        {
            Kullanici? kullanici = await context.Kullanicis.Where(k => k.KullaniciAdi == MevcutKullaniciAdi).AsNoTracking().FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Kullanıcı Bulunamadı");

            return mapper.Map<KullaniciGetirDTO>(kullanici);
        }

        public async Task<IEnumerable<KullaniciGetirDTO>> TumDigerKullanicilariGetir(CancellationToken cancellationToken)
        {
            IEnumerable<Kullanici> kullanicilar = await context.Kullanicis.Where(k => k.KullaniciAdi != MevcutKullaniciAdi).AsNoTracking().ToListAsync(cancellationToken);

            if (!kullanicilar.Any()) throw new Exception("Kullanıcı Bulunamadı");
            return mapper.Map<IEnumerable<KullaniciGetirDTO>>(kullanicilar);
        }
    }
}
