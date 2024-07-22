using MediatR;

namespace ChatAppAPI.Kullanicilar.Queries.TumKullanicilariGetir
{
    public class TumKullanicilariGetirRequest : IRequest<IList<TumKullanicilariGetirResponse>>
    {
    }
}
