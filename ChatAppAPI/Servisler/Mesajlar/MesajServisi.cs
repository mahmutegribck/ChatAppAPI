using AutoMapper;
using ChatAppAPI.Context;
using ChatAppAPI.Models;
using ChatAppAPI.Servisler.Kullanicilar;
using ChatAppAPI.Servisler.Mesajlar.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Servisler.Mesajlar
{
    public class MesajServisi(ChatAppDbContext context, IMapper mapper, IKullaniciServisi kullaniciServisi) : IMesajServisi
    {
        public async Task MesajEkle(MesajGonderDTO messageDto)
        {
            Kullanici? alici = await context.Kullanicis
                .Where(k => k.KullaniciAdi == messageDto.AliciAdi)
                .AsNoTracking()
                .FirstOrDefaultAsync() ?? throw new Exception("Alıcı Bulunamadı");

            Kullanici? gönderici = await context.Kullanicis
                .Where(k => k.KullaniciAdi == kullaniciServisi.MevcutKullaniciAdi)
                .AsNoTracking()
                .FirstOrDefaultAsync() ?? throw new Exception("Gönderici Bulunamadı.");

            Mesaj mesaj = new()
            {
                Text = messageDto.Text,
                GonderilmeZamani = messageDto.GonderilmeZamani,
                GonderenId = gönderici.Id,
                AliciId = alici.Id
            };

            await context.Mesajs.AddAsync(mesaj);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MesajGetirDTO>> MesajlariGetir(string aliciKullaniciAdi, int sayfaBuyuklugu, int sayfaNumarasi, CancellationToken cancellationToken)
        {
            string? mevcutKullanici = kullaniciServisi.MevcutKullaniciAdi ?? throw new Exception("Kullanıcı Bulunamadı");

            if (!await context.Kullanicis.AnyAsync(k => k.KullaniciAdi == aliciKullaniciAdi, cancellationToken)) throw new Exception("Alıcı Kullanıcı Bulunamadı");

            IEnumerable<Mesaj> mesajlar = await context.Mesajs
                .Include(m => m.Gonderen)
                .Include(m => m.Alici)
                .Where(m =>
                    m.Gonderen.KullaniciAdi == mevcutKullanici &&
                    m.Alici.KullaniciAdi == aliciKullaniciAdi ||
                    m.Gonderen.KullaniciAdi == aliciKullaniciAdi &&
                    m.Alici.KullaniciAdi == mevcutKullanici)
                .OrderBy(m => m.GonderilmeZamani).Skip((sayfaNumarasi - 1) * sayfaBuyuklugu).Take(sayfaBuyuklugu)
                .ToListAsync(cancellationToken);

            if (!mesajlar.Any()) throw new Exception("Mesaj Bulunamadı");

            await MesajlariGorulduYap(mesajlar.Where(m => m.Alici.KullaniciAdi == mevcutKullanici && m.GorulmeDurumu == false).ToList(), cancellationToken);

            return mapper.Map<IEnumerable<MesajGetirDTO>>(mesajlar);
        }

        public async Task MesajlariGorulduYap(IEnumerable<Mesaj> mesajlar, CancellationToken cancellationToken)
        {
            foreach (var mesaj in mesajlar)
            {
                mesaj.GorulmeDurumu = true;
            }
            await context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<object>> MesajlasilanKullanicilariGetir(CancellationToken cancellationToken)
        {
            Kullanici? kullanici = await context.Kullanicis
               .Where(k => k.KullaniciAdi == kullaniciServisi.MevcutKullaniciAdi)
               .AsNoTracking()
               .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception("Kullanıcı Bulunamadı");


            var mesajlasilanKullanicilar = await context.Mesajs
                .Where(m => m.GonderenId == kullanici.Id || m.AliciId == kullanici.Id)
                .Select(m => m.GonderenId == kullanici.Id ? m.AliciId : m.GonderenId)
                .Distinct()
                .Select(kullaniciAdi => new
                {
                    MesajlasilanKullanici = context.Kullanicis.Where(u => u.Id == kullaniciAdi).Select(u => new
                    {
                        KullaniciAdi = u.KullaniciAdi,
                        ProfilResmiUrl = u.ProfileImageUrl
                    }).AsNoTracking().FirstOrDefault(),

                    Mesaj = context.Mesajs.Where(msg => (msg.GonderenId == kullanici.Id && msg.AliciId == kullaniciAdi) || (msg.GonderenId == kullaniciAdi && msg.AliciId == kullanici.Id))
                    .OrderByDescending(msg => msg.GonderilmeZamani)
                    .Select(msg => new
                    {
                        SonMesajGonderenAdi = msg.Gonderen.KullaniciAdi,
                        SonGonderilenMesaj = msg.Text,
                        SonGonderilenMesajTarihi = msg.GonderilmeZamani.ToShortDateString(),
                        SonGonderilenMesajSaati = msg.GonderilmeZamani.ToShortTimeString(),

                    })
                    .AsNoTracking()
                    .FirstOrDefault(),
                    GorulmeyenMesajSayisi = context.Mesajs.Count(msg => msg.AliciId == kullanici.Id && msg.GonderenId == kullaniciAdi && !msg.GorulmeDurumu)
                })
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            if (mesajlasilanKullanicilar.Count == 0) throw new Exception("Mesajlaşılan Kullanıcı Bulunamadı");

            return mesajlasilanKullanicilar;
        }
    }
}
