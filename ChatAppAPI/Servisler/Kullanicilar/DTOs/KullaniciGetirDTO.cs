namespace ChatAppAPI.Servisler.Kullanicilar.DTOs
{
    public class KullaniciGetirDTO
    {
        public required string KullaniciAdi { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
