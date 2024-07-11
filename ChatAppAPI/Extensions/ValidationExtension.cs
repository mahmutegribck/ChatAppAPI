using ChatAppAPI.Servisler.Mesajlar.DTOs;
using ChatAppAPI.Servisler.OturumYonetimi.DTOs;
using FluentValidation;

namespace ChatAppAPI.Extensions
{
    public static class ValidationExtension
    {
        public static IServiceCollection AddValidationExtension(this IServiceCollection services)
        {
            services.AddScoped<IValidator<MesajGonderDTO>, MesajGonderDTOValidator>();
            services.AddScoped<IValidator<MesajlariGorulduYapDTO>, MesajlariGorulduYapDTOValidator>();

            services.AddScoped<IValidator<KullaniciKayitDto>, KullaniciKayitDtoValidator>();
            services.AddScoped<IValidator<KullaniciGirisDto>, KullaniciGirisDtoValidator>();
            services.AddScoped<IValidator<KullaniciAdiIleGirisYapDTO>, KullaniciAdiIleGirisYapDTOValidator>();

            return services;
        }
    }
}
