using ChatAppAPI.Hubs;
using ChatAppAPI.Servisler.Mesajlar;
using ChatAppAPI.Servisler.Mesajlar.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ChatAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MesajController(IMesajServisi mesajServisi, IHubContext<ChatHub> hubContext) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> MesajGonder([FromBody] MesajGonderDTO mesajGonderDTO, CancellationToken cancellationToken)
        {
            await mesajServisi.MesajEkle(mesajGonderDTO, cancellationToken);

            if (ChatHub.BagliKullaniciAdlari.Contains(mesajGonderDTO.AliciAdi))
            {
                await hubContext.Clients
                    .Group(mesajGonderDTO.AliciAdi)
                        .SendAsync("messageToUserReceived", JsonConvert.SerializeObject(mesajGonderDTO), cancellationToken);
            }
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> MesajlariGetir([FromQuery, Required] string aliciAdi, [FromHeader, Required] int sayfaBuyuklugu, [FromHeader, Required] int sayfaNumarasi, CancellationToken cancellationToken)
        {
            IEnumerable<MesajGetirDTO> mesajlar = await mesajServisi.MesajlariGetir(aliciAdi, sayfaBuyuklugu, sayfaNumarasi, cancellationToken);

            return Ok(mesajlar);
        }


        [HttpPatch]
        public async Task<IActionResult> MesajlariGorulduYap([FromBody] MesajlariGorulduYapDTO mesajlariGorulduYapDTO, CancellationToken cancellationToken)
        {
            await mesajServisi.MesajlariGorulduYap(mesajlariGorulduYapDTO, cancellationToken);
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> MesajlasilanKullanicilariGetir(CancellationToken cancellationToken)
        {
            IEnumerable<object> mesajlaşılanKullanıcılar = await mesajServisi.MesajlasilanKullanicilariGetir(cancellationToken);

            return Ok(mesajlaşılanKullanıcılar);
        }
    }
}
