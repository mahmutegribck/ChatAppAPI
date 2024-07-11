using ChatAppAPI.ExceptionHandling.Exceptions;
using ChatAppAPI.Servisler.Mesajlar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppAPI.Hubs
{
    [Authorize]
    public class ChatHub(IMesajServisi mesajServisi) : Hub
    {
        public static List<string> BagliKullaniciAdlari { get; } = [];

        //public async Task SendMessageToUser(MesajGonderDTO messageDto)
        //{
        //    if (BagliKullaniciIdler.Contains(messageDto.AliciAdi))
        //    {
        //        await Clients.Group(messageDto.AliciAdi).SendAsync("messageToUserReceived", JsonConvert.SerializeObject(messageDto));
        //    }
        //    await mesajServisi.MesajEkle(messageDto);
        //}

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
