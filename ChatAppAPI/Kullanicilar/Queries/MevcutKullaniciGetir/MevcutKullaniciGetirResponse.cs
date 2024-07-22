namespace ChatAppAPI.Kullanicilar.Queries.MevcutKullaniciGetir
{
    public class MevcutKullaniciGetirResponse
    {
        public required string Id { get; set; }
        public required string KullaniciAdi { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
