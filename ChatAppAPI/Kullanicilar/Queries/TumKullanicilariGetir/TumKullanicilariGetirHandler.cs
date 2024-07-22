using ChatAppAPI.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Kullanicilar.Queries.TumKullanicilariGetir
{
    public class TumKullanicilariGetirHandler(ChatAppDbContext context, IHttpContextAccessor httpContextAccessor) : IRequestHandler<TumKullanicilariGetirRequest, IList<TumKullanicilariGetirResponse>>
    {
        public async Task<IList<TumKullanicilariGetirResponse>> Handle(TumKullanicilariGetirRequest request, CancellationToken cancellationToken)
        {
            var mevcutKullaniciAdi = (httpContextAccessor.HttpContext?.User?.Identity?.Name) ?? throw new Exception("Mevcut Kullanici Bulunamadi.");

            var kullanicilar = await context.Kullanicis.Where(k => k.KullaniciAdi != mevcutKullaniciAdi).ToListAsync(cancellationToken);

            if (kullanicilar.Count == 0) throw new Exception("Kullanici Bulunamadi.");

            IList<TumKullanicilariGetirResponse> responseList = [];

            foreach (var kullanici in kullanicilar)
            {
                TumKullanicilariGetirResponse response = new()
                {
                    Id = kullanici.Id,
                    KullaniciAdi = kullanici.KullaniciAdi,
                    ProfileImageUrl = kullanici.ProfileImageUrl,
                };

                responseList.Add(response);
            }
            return responseList;
        }
    }
}
