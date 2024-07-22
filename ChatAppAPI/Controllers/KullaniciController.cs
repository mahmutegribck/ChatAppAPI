using ChatAppAPI.Kullanicilar.Queries.MevcutKullaniciGetir;
using ChatAppAPI.Kullanicilar.Queries.TumKullanicilariGetir;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class KullaniciController(IMediator mediator) : ControllerBase
    {
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        [HttpGet]
        public async Task<IActionResult> MevcutKullaniciGetir(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new MevcutKullaniciGetirRequest(), cancellationToken);
            return Ok(response);
        }


        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
        [HttpGet]
        public async Task<IActionResult> TumDigerKullanicilariGetir(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new TumKullanicilariGetirRequest(), cancellationToken);
            return Ok(response);
        }
    }
}
