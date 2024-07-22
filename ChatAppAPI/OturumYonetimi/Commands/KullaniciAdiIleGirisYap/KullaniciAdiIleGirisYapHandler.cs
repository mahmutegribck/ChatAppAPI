using ChatAppAPI.Context;
using ChatAppAPI.Models;
using ChatAppAPI.Servisler.OturumYonetimi.JWT;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ChatAppAPI.OturumYonetimi.Commands.KullaniciAdiIleGirisYap
{
    public class KullaniciAdiIleGirisYapHandler(ChatAppDbContext context, IJwtServisi jwtServisi) : IRequestHandler<KullaniciAdiIleGirisYapRequest, KullaniciAdiIleGirisYapResponse>
    {
        public async Task<KullaniciAdiIleGirisYapResponse> Handle(KullaniciAdiIleGirisYapRequest request, CancellationToken cancellationToken)
        {
            Kullanici? kullanici = await context.Kullanicis.Where(k => k.KullaniciAdi == request.KullaniciAdi).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            if (kullanici == null)
            {
                var byteArray = Encoding.Default.GetBytes(Guid.NewGuid().ToString());
                var hashedSifre = Convert.ToBase64String(SHA256.HashData(byteArray));

                Kullanici yeniKullanici = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    KullaniciAdi = request.KullaniciAdi,
                    KullaniciSifresi = hashedSifre
                };
                await context.Kullanicis.AddAsync(yeniKullanici, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

            }
            string? token = jwtServisi.KullaniciAdiIleTokenOlustur(request.KullaniciAdi);

            return new KullaniciAdiIleGirisYapResponse
            {
                AccessToken = token
            };
        }
    }
}
