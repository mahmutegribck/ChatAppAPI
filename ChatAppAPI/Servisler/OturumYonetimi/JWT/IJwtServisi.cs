using ChatAppAPI.Models;

namespace ChatAppAPI.Servisler.OturumYonetimi.JWT
{
    public interface IJwtServisi
    {
        string JwtTokenOlustur(Kullanici kullanici);
        string? KullaniciAdiIleTokenOlustur(string kullaniciAdi);

    }
}
