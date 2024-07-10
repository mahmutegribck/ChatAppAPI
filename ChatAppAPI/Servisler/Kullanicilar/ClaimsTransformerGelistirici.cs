using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace ChatAppAPI.Servisler.Kullanicilar
{
    public class ClaimsTransformerGelistirici(IKullaniciServisi kullaniciServisi) : IClaimsTransformation
    {
        public class TokenKullaniciBilgisiGelistirici
        {
            public string KullaniciAdi { get; set; } = null!;

            public TokenKullaniciBilgisiGelistirici(ClaimsIdentity claimsIdentity)
            {
                if (claimsIdentity.HasClaim(x => x.Type == "kullaniciAdi"))
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    claimsIdentity.AddClaim(
                        new Claim(
                            ClaimTypes.Name,
                            KullaniciAdi = claimsIdentity.FindFirst(x => x.Type == "kullaniciAdi").Value
                        )
                    );
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                else
                    throw new Exception("Kullanıcı adı hatası");
            }
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            try
            {
                ClaimsIdentity? claimsIdentity = (ClaimsIdentity?)principal.Identity;

                if (claimsIdentity != null && claimsIdentity.IsAuthenticated)
                {
                    TokenKullaniciBilgisiGelistirici tokenKullaniciBilgisi;
                    try
                    {
                        tokenKullaniciBilgisi = new TokenKullaniciBilgisiGelistirici(claimsIdentity);

                        var kullaniciBilgi = kullaniciServisi.KullaniciGetir(
                            tokenKullaniciBilgisi.KullaniciAdi
                        );
                        if (kullaniciBilgi == null)
                            throw new Exception("Kullanıcı hatası");
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Task.FromResult(principal);
        }
    }
}
