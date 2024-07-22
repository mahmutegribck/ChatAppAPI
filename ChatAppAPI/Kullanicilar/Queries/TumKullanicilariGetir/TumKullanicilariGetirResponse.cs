namespace ChatAppAPI.Kullanicilar.Queries.TumKullanicilariGetir
{
    public class TumKullanicilariGetirResponse
    {
        public required string Id { get; set; }
        public required string KullaniciAdi { get; set; }
        public string? ProfileImageUrl { get; set; }

    }
}
