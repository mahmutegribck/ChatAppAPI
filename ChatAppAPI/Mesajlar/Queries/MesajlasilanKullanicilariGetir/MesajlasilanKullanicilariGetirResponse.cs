namespace ChatAppAPI.Mesajlar.Queries.MesajlasilanKullanicilariGetir
{
    public class MesajlasilanKullanicilariGetirResponse
    {
        public required string KullaniciAdi { get; set; }
        public string? ProfilResmiUrl { get; set; }
        public required string SonMesajGonderenAdi { get; set; }
        public required string SonGonderilenMesaj { get; set; }
        public required string SonGonderilenMesajTarihi { get; set; }
        public required string SonGonderilenMesajSaati { get; set; }
        public int GorulmeyenMesajSayisi { get; set; }

    }
}
