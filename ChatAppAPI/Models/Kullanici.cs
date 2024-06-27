using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Models
{
    public class Kullanici
    {
        [Key]
        public required string Id { get; set; }
        public required string KullaniciAdi { get; set; }
        public required string KullaniciSifresi { get; set; }

        public string? ProfileImageUrl { get; set; }
        public string? ProfileImagePath { get; set; }

        public ICollection<Mesaj> GonderilenMesajlar { get; set; }
        public ICollection<Mesaj> AlinanMesajlar { get; set; }
    }
}
