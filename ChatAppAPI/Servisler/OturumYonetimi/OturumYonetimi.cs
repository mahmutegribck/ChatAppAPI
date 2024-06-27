using AutoMapper;
using ChatAppAPI.Context;
using ChatAppAPI.Models;
using ChatAppAPI.Servisler.OturumYonetimi.DTOs;
using ChatAppAPI.Servisler.OturumYonetimi.JWT;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ChatAppAPI.Servisler.OturumYonetimi
{
    public class OturumYonetimi(IMapper mapper, IJwtServisi jwtServisi, ChatAppDbContext context) : IOturumYonetimi
    {
        public async Task<string?> GirisYap(KullaniciGirisDto model)
        {
            try
            {
                var byteArray = Encoding.Default.GetBytes(model.KullaniciSifresi);
                var hashedSifre = Convert.ToBase64String(SHA256.HashData(byteArray));

                model.KullaniciSifresi = hashedSifre;
                model.KullaniciAdi = model.KullaniciAdi.Trim().ToLower();

                Kullanici? kullanici = await context.Kullanicis.Where(k => k.KullaniciAdi == model.KullaniciAdi && k.KullaniciSifresi == model.KullaniciSifresi).AsNoTracking().FirstOrDefaultAsync();
                if (kullanici == null)
                {
                    return null;
                }

                string token = jwtServisi.JwtTokenOlustur(kullanici);

                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task KayitOl(KullaniciKayitDto model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model), "Model boş olamaz.");
                }

                var mevcutKullanici = await context.Kullanicis.Where(k => k.KullaniciAdi == model.KullaniciAdi).AsNoTracking().FirstOrDefaultAsync();

                if (mevcutKullanici != null)
                {
                    throw new ArgumentException("Bu kullanıcı adı mevcut.", model.KullaniciAdi);
                }
                model.KullaniciAdi = model.KullaniciAdi.Trim().ToLower();
                Kullanici yeniKullanici = mapper.Map<Kullanici>(model);
                yeniKullanici.Id = Guid.NewGuid().ToString();
                var byteArray = Encoding.Default.GetBytes(yeniKullanici.KullaniciSifresi);
                var hashedSifre = Convert.ToBase64String(SHA256.HashData(byteArray));

                yeniKullanici.KullaniciSifresi = hashedSifre;

                await context.Kullanicis.AddAsync(yeniKullanici);
                await context.SaveChangesAsync();

            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }

        public async Task<string?> KullaniciAdiIleGirisYap(string kullaniciAdi, CancellationToken cancellationToken)
        {
            try
            {
                Kullanici? kullanici = await context.Kullanicis.Where(k => k.KullaniciAdi == kullaniciAdi).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (kullanici == null)
                {
                    var byteArray = Encoding.Default.GetBytes(Guid.NewGuid().ToString());
                    var hashedSifre = Convert.ToBase64String(SHA256.HashData(byteArray));

                    Kullanici yeniKullanici = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        KullaniciAdi = kullaniciAdi,
                        KullaniciSifresi = hashedSifre
                    };
                    await context.Kullanicis.AddAsync(yeniKullanici,cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                }
                string? token = jwtServisi.KullaniciAdiIleTokenOlustur(kullaniciAdi);

                return token;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
