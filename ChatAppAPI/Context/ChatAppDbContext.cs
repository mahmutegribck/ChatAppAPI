using ChatAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppAPI.Context
{
    public class ChatAppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Mesaj> Mesajs { get; set; }
        public DbSet<Kullanici> Kullanicis { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Kullanici>()
                .HasIndex(u => u.KullaniciAdi)
                .IsUnique();

            modelBuilder.Entity<Mesaj>()
            .HasOne(m => m.Gonderen)
            .WithMany(u => u.GonderilenMesajlar)
            .HasForeignKey(m => m.GonderenId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mesaj>()
                .HasOne(m => m.Alici)
                .WithMany(u => u.AlinanMesajlar)
                .HasForeignKey(m => m.AliciId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
