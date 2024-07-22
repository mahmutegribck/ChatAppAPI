using MediatR;

namespace ChatAppAPI.Mesajlar.Queries.MesajlasilanKullanicilariGetir
{
    public class MesajlasilanKullanicilariGetirRequest : IRequest<IList<MesajlasilanKullanicilariGetirResponse>>
    {
    }
}
