using ChatAppAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatAppAPI.Servisler.OturumYonetimi.JWT
{
    public class JwtServisi(IConfiguration configuration) : IJwtServisi
    {
        public string JwtTokenOlustur(Kullanici kullanici)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key =
                new(Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? string.Empty));


            var claims = new List<Claim>
            {
                new("kullaniciAdi", kullanici.KullaniciAdi),
            };
            var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = configuration["Jwt:Audience"],
                Issuer = configuration["Jwt:Issuer"],
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenhandler.CreateToken(tokendesc);
            var finaltoken = tokenhandler.WriteToken(token);
            return finaltoken;
        }

        public string? KullaniciAdiIleTokenOlustur(string kullaniciAdi)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key =
                new(Encoding.UTF8.GetBytes(configuration["JWT:Key"] ?? string.Empty));

            var claims = new List<Claim>
            {
                new("kullaniciAdi", kullaniciAdi),
            };

            var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = configuration["Jwt:Audience"],
                Issuer = configuration["Jwt:Issuer"],
                //Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenhandler.CreateToken(tokendesc);
            var finaltoken = tokenhandler.WriteToken(token);
            return finaltoken;
        }
    }
}
