namespace ChatAppAPI.Servisler.Mesajlar.DTOs
{
    public class MesajGonderDTO
    {
        public required string Text { get; set; }
        public required string AliciAdi { get; set; }
        public DateTime GonderilmeZamani { get; set; } = DateTime.Now;

    }
}
