using MediatR;
using Servicos.CalculoImposto.Application.Models;

namespace Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId
{
    public sealed record PegarPeloPedidoIdQuery : IRequest<RespostaPadronizadaModel>
    {
        public PegarPeloPedidoIdQuery(int pedidoId)
        {
            PedidoId = pedidoId;
        }

        public int PedidoId { get; set; }
    }
}