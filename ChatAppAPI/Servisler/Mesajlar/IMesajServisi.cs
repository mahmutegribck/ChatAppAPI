using ChatAppAPI.Models;
using ChatAppAPI.Servisler.Mesajlar.DTOs;

namespace ChatAppAPI.Servisler.Mesajlar
{
    public interface IMesajServisi
    {
        Task MesajEkle(MesajGonderDTO messageDto);
        Task<IEnumerable<MesajGetirDTO>> MesajlariGetir(string aliciKullaniciAdi, int sayfaBuyuklugu, int sayfaNumarasi, CancellationToken cancellationToken);
        Task MesajlariGorulduYap(List<int> mesajIds, CancellationToken cancellationToken);
        Task<IEnumerable<object>> MesajlasilanKullanicilariGetir(CancellationToken cancellationToken);

    }
}
