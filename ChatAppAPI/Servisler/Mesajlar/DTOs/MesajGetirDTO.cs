namespace ChatAppAPI.Servisler.Mesajlar.DTOs
{
    public class MesajGetirDTO
    {
        public required string GondericiAdi { get; set; }
        public required string AliciAdi { get; set; }
        public string? Text { get; set; }
        public required string GonderilmeTarihi { get; set; }
        public required string GonderilmeSaati { get; set; }

    }
}
