using ChatAppAPI.Servisler.Mesajlar.DTOs;

namespace ChatAppAPI.Servisler.Mesajlar
{
    public interface IMesajServisi
    {
        Task MesajEkle(MesajGonderDTO messageDto, CancellationToken cancellationToken);
        Task<IEnumerable<MesajGetirDTO>> MesajlariGetir(string aliciKullaniciAdi, int sayfaBuyuklugu, int sayfaNumarasi, CancellationToken cancellationToken);
        Task MesajlariGorulduYap(MesajlariGorulduYapDTO mesajlariGorulduYapDTO, CancellationToken cancellationToken);
        Task<IEnumerable<object>> MesajlasilanKullanicilariGetir(CancellationToken cancellationToken);

    }
}
