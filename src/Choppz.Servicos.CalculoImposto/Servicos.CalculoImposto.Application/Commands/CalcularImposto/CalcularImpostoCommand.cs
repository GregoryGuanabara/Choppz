using MediatR;
using Servicos.CalculoImposto.Application.Models;

namespace Servicos.CalculoImposto.Application.Commands.CalcularImposto
{
    public sealed record CalcularImpostoCommand : IRequest<RespostaPadronizadaModel>
    {
        public CalcularImpostoCommand(int pedidoId, int clienteId, List<PedidoItemModel> itens)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Itens = itens;
        }

        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public List<PedidoItemModel> Itens { get; set; }

        public decimal CalcularValorTotal()
        {
            return Itens?.Sum(x => x.Valor * x.Quantidade) ?? 0;
        }
    }
}