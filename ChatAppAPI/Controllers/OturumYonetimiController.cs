using ChatAppAPI.Servisler.OturumYonetimi;
using ChatAppAPI.Servisler.OturumYonetimi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class OturumYonetimiController(IOturumYonetimi oturumYonetimi) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> KayitOl([FromBody, Required] KullaniciKayitDto model)
        {
            if (ModelState.IsValid)
            {
                await oturumYonetimi.KayitOl(model);
                return Ok("Kullanici Kaydi Basarili");
            }
            return BadRequest("Kullanici Kaydi Basarisiz");
        }


        [HttpPost]
        public async Task<IActionResult> GirisYap([FromBody, Required] KullaniciGirisDto model)
        {
            string? token = await oturumYonetimi.GirisYap(model);

            if (token != null)
            {
                return Ok(token);
            }
            return NotFound("Giriş Bulunamadi");
        }


        [HttpPost]
        public async Task<IActionResult> KullaniciAdiIleGirisYap(string kullaniciAdi, CancellationToken cancellationToken)
        {
            string? token = await oturumYonetimi.KullaniciAdiIleGirisYap(kullaniciAdi, cancellationToken);

            if (token != null)
            {
                return Ok(token);
            }
            return NotFound("Giriş Yapılamadı");
        }

    }
}