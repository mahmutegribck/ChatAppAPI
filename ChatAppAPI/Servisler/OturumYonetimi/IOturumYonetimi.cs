using ChatAppAPI.Servisler.OturumYonetimi.DTOs;

namespace ChatAppAPI.Servisler.OturumYonetimi
{
    public interface IOturumYonetimi
    {
        Task KayitOl(KullaniciKayitDto model);
        Task<string?> GirisYap(KullaniciGirisDto model);
        Task<string?> KullaniciAdiIleGirisYap(string kullaniciAdi, CancellationToken cancellationToken);

    }
}
