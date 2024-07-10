using ChatAppAPI.Servisler.Mesajlar;
using ChatAppAPI.Servisler.Mesajlar.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ChatAppAPI.Hubs
{
    [Authorize]
    public class ChatHub(IMesajServisi mesajServisi) : Hub
    {
        public static List<string> BagliKullaniciIdler { get; } = [];

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
            var kullaniciAdi = (Context.GetHttpContext()!.User?.Identity?.Name) ?? throw new Exception("Kullanıcı bulunamadı.");

            lock (BagliKullaniciIdler)
            {
                if (!BagliKullaniciIdler.Contains(kullaniciAdi))
                    BagliKullaniciIdler.Add(kullaniciAdi);
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, kullaniciAdi);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var kullaniciAdi = Context.GetHttpContext()!.User?.Identity?.Name ?? throw new Exception("Kullanıcı bulunamadı.");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, kullaniciAdi);

            lock (BagliKullaniciIdler)
            {
                BagliKullaniciIdler.Remove(kullaniciAdi);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
