using ChatAppAPI.Context;
using ChatAppAPI.Models;
using ChatAppAPI.Servisler.OturumYonetimi.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ChatAppAPI.OturumYonetimi.Commands.GirisYap
{
    public class GirisYapHandler(ChatAppDbContext context, IJwtServisi jwtServisi) : IRequestHandler<GirisYapRequest, GirisYapResponse>
    {
        public async Task<GirisYapResponse> Handle(GirisYapRequest request, CancellationToken cancellationToken)
        {
            var byteArray = Encoding.Default.GetBytes(request.KullaniciSifresi);
            var hashedSifre = Convert.ToBase64String(SHA256.HashData(byteArray));

            Kullanici? kullanici = await context.Kullanicis
                .Where(k => k.KullaniciAdi == request.KullaniciAdi.Trim().ToLower() && k.KullaniciSifresi == hashedSifre)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Kullanıcı Bulunamadı.");

            string token = jwtServisi.JwtTokenOlustur(kullanici);

            return new GirisYapResponse
            {
                AccessToken = token
            };
        }
    }
}
