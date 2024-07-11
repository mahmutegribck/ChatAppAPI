using ChatAppAPI.Servisler.Kullanicilar;
using ChatAppAPI.Servisler.Kullanicilar.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class KullaniciController(IKullaniciServisi kullaniciServisi) : ControllerBase
    {
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        [HttpGet]
        public async Task<IActionResult> MevcutKullaniciGetir(CancellationToken cancellationToken)
        {
            KullaniciGetirDTO? kullanici = await kullaniciServisi.MevcutKullaniciGetir(cancellationToken);
            return Ok(kullanici);
        }


        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        [HttpGet]
        public async Task<IActionResult> TumDigerKullanicilariGetir(CancellationToken cancellationToken)
        {
            IEnumerable<KullaniciGetirDTO> kullanicilar = await kullaniciServisi.TumDigerKullanicilariGetir(cancellationToken);
            return Ok(kullanicilar);
        }
    }
}
