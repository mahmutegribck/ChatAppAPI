using ChatAppAPI.Servisler.OturumYonetimi;
using ChatAppAPI.Servisler.OturumYonetimi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class OturumYonetimiController(IOturumYonetimi oturumYonetimi) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> KayitOl([FromBody] KullaniciKayitDto model, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid)
            {
                await oturumYonetimi.KayitOl(model, cancellationToken);
                return Ok("Kullanici Kaydi Basarili");
            }
            return BadRequest("Kullanici Kaydi Basarisiz");
        }


        [HttpPost]
        public async Task<IActionResult> GirisYap([FromBody] KullaniciGirisDto model, CancellationToken cancellationToken)
        {
            string? token = await oturumYonetimi.GirisYap(model, cancellationToken);

            if (token != null)
            {
                return Ok(token);
            }
            return NotFound("Giriş Yapılamadı");
        }


        [HttpPost]
        public async Task<IActionResult> KullaniciAdiIleGirisYap([FromBody] KullaniciAdiIleGirisYapDTO kullaniciAdiIleGirisYapDTO, CancellationToken cancellationToken)
        {
            string? token = await oturumYonetimi.KullaniciAdiIleGirisYap(kullaniciAdiIleGirisYapDTO.KullaniciAdi, cancellationToken);

            if (token != null)
            {
                return Ok(token);
            }
            return NotFound("Giriş Yapılamadı");
        }
    }
}