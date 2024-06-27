using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Models
{
    public class Mesaj
    {
        [Key]
        public int Id { get; set; }
        public required string Text { get; set; }
        public DateTime GonderilmeZamani { get; set; }
        public bool GorulmeDurumu { get; set; } = false;
        public required string GonderenId { get; set; }
        public Kullanici Gonderen { get; set; } = null!;
        public required string AliciId { get; set; }
        public Kullanici Alici { get; set; } = null!;

    }
}
