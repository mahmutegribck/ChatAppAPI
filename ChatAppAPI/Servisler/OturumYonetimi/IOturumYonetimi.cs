using ChatAppAPI.Servisler.OturumYonetimi.DTOs;

namespace ChatAppAPI.Servisler.OturumYonetimi
{
    public interface IOturumYonetimi
    {
        Task KayitOl(KullaniciKayitDto model, CancellationToken cancellationToken);
        Task<string?> GirisYap(KullaniciGirisDto model, CancellationToken cancellationToken);
        Task<string?> KullaniciAdiIleGirisYap(string kullaniciAdi, CancellationToken cancellationToken);

    }
}
