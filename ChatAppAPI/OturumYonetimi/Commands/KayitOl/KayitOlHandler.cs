using ChatAppAPI.Context;
using ChatAppAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ChatAppAPI.OturumYonetimi.Commands.KayitOl
{
    public class KayitOlHandler(ChatAppDbContext context) : IRequestHandler<KayitOlRequest>
    {
        public async Task Handle(KayitOlRequest request, CancellationToken cancellationToken)
        {
            var mevcutKullanici = await context.Kullanicis
                  .Where(k => k.KullaniciAdi == request.KullaniciAdi)
                  .AsNoTracking()
                  .FirstOrDefaultAsync(cancellationToken);

            if (mevcutKullanici != null)
            {
                throw new ArgumentException("Bu kullanıcı adı mevcut.", request.KullaniciAdi);
            }

            Kullanici yeniKullanici = new()
            {
                Id = Guid.NewGuid().ToString(),
                KullaniciAdi = request.KullaniciAdi,
                KullaniciSifresi = request.KullaniciSifresi
            };

            var byteArray = Encoding.Default.GetBytes(yeniKullanici.KullaniciSifresi);
            var hashedSifre = Convert.ToBase64String(SHA256.HashData(byteArray));

            yeniKullanici.KullaniciSifresi = hashedSifre;
            yeniKullanici.KullaniciAdi = yeniKullanici.KullaniciAdi.Trim().ToLower();

            await context.Kullanicis.AddAsync(yeniKullanici, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
