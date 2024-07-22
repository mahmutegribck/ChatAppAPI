using ChatAppAPI.Context;
using ChatAppAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Kullanicilar.Queries.MevcutKullaniciGetir
{
    public class MevcutKullaniciGetirHandler(ChatAppDbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<MevcutKullaniciGetirRequest, MevcutKullaniciGetirResponse>
    {
        public async Task<MevcutKullaniciGetirResponse> Handle(MevcutKullaniciGetirRequest request, CancellationToken cancellationToken)
        {
            var mevcutKullaniciAdi = (httpContextAccessor.HttpContext?.User?.Identity?.Name) ?? throw new Exception("Mevcut Kullanici Bulunamadi.");

            Kullanici? mevcutKullanici = await context.Kullanicis.Where(k => k.KullaniciAdi == mevcutKullaniciAdi).FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Kullanici Bulunamadi.");

            return new MevcutKullaniciGetirResponse
            {
                Id = mevcutKullanici.Id,
                KullaniciAdi = mevcutKullanici.KullaniciAdi,
                ProfileImageUrl = mevcutKullanici.ProfileImageUrl,
            };
        }
    }
}
