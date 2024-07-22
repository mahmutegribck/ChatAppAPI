using ChatAppAPI.ExceptionHandling.Exceptions;
using ChatAppAPI.Mesajlar.Commands.MesajEkle;
using ChatAppAPI.Servisler.Mesajlar.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ChatAppAPI.Hubs
{
    [Authorize]
    public class ChatHub(IMediator mediator) : Hub
    {
        public static List<string> BagliKullaniciAdlari { get; } = [];

        public async Task SendMessageToUser(MesajGonderDTO mesajGonderDTO)
        {
            if (BagliKullaniciAdlari.Contains(mesajGonderDTO.AliciAdi))
            {
                await Clients.Group(mesajGonderDTO.AliciAdi).SendAsync("messageToUserReceived", JsonConvert.SerializeObject(mesajGonderDTO));
            }
            await mediator.Send(new MesajEkleRequest
            {
                Text = mesajGonderDTO.Text,
                AliciAdi = mesajGonderDTO.AliciAdi
            });
        }

        public override async Task OnConnectedAsync()
        {
            var kullaniciAdi = (Context.GetHttpContext()!.User?.Identity?.Name) ?? throw new NotFoundException("Kullanıcı bulunamadı.");

            lock (BagliKullaniciAdlari)
            {
                if (!BagliKullaniciAdlari.Contains(kullaniciAdi))
                    BagliKullaniciAdlari.Add(kullaniciAdi);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, kullaniciAdi);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var kullaniciAdi = Context.GetHttpContext()!.User?.Identity?.Name ?? throw new NotFoundException("Kullanıcı bulunamadı.");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, kullaniciAdi);

            lock (BagliKullaniciAdlari)
            {
                BagliKullaniciAdlari.Remove(kullaniciAdi);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
