using ChatAppAPI.Mesajlar.Commands.MesajlariGorulduYap;
using ChatAppAPI.Mesajlar.Queries.MesajlariGetir;
using ChatAppAPI.Mesajlar.Queries.MesajlasilanKullanicilariGetir;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ChatAppAPI.Hubs;
using ChatAppAPI.Servisler.Mesajlar.DTOs;
using ChatAppAPI.Servisler.Mesajlar;
using Newtonsoft.Json;
using Microsoft.AspNetCore.SignalR;


namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class MesajController(IMediator mediator, IMesajServisi mesajServisi, IHubContext<ChatHub> hubContext) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> MesajGonder([FromBody] MesajGonderDTO mesajGonderDTO, CancellationToken cancellationToken)
        {
            if (ChatHub.BagliKullaniciAdlari.Contains(mesajGonderDTO.AliciAdi))
            {
                if (cancellationToken.IsCancellationRequested)
                    return BadRequest();

                await hubContext.Clients
                    .Group(mesajGonderDTO.AliciAdi)
                        .SendAsync("messageToUserReceived", JsonConvert.SerializeObject(mesajGonderDTO), cancellationToken);
            }
            await mesajServisi.MesajEkle(mesajGonderDTO, cancellationToken);

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> MesajlariGetir([FromBody] MesajlariGetirRequest request, CancellationToken cancellationToken)
        {
            var response = await mediator.Send(request, cancellationToken);
            return Ok(response);
        }


        [HttpPatch]
        public async Task<IActionResult> MesajlariGorulduYap([FromBody] MesajlariGorulduYapRequest request, CancellationToken cancellationToken)
        {
            await mediator.Send(request, cancellationToken);
            return Ok();
        }
         

        [HttpGet]
        public async Task<IActionResult> MesajlasilanKullanicilariGetir(CancellationToken cancellationToken)
        {
            var response = await mediator.Send(new MesajlasilanKullanicilariGetirRequest(), cancellationToken);
            return Ok(response);
        }
    }
}

