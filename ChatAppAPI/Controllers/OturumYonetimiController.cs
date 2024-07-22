using ChatAppAPI.OturumYonetimi.Commands.GirisYap;
using ChatAppAPI.OturumYonetimi.Commands.KayitOl;
using ChatAppAPI.OturumYonetimi.Commands.KullaniciAdiIleGirisYap;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class OturumYonetimiController(IMediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> KayitOl([FromBody] KayitOlRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> GirisYap([FromBody] GirisYapRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> KullaniciAdiIleGirisYap([FromBody] KullaniciAdiIleGirisYapRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}