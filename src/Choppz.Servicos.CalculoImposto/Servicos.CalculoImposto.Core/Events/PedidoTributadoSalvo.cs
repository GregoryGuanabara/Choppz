using Servicos.CalculoImposto.Core.BaseEntities;
using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Events
{
    public sealed record PedidoTributadoSalvo : IDomainEvent
    {
        public PedidoTributadoSalvo(int pedidoComImpostoId, int pedidoId, int clienteId, decimal imposto, List<PedidoItemDTO> itens, EPedidoTributadoStatus status)
        {
            OcorridoEm = DateTime.Now;
            PedidoComImpostoId = pedidoComImpostoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Imposto = imposto;
            Itens = itens;
            Status = status;
        }

        public DateTime OcorridoEm { get; private set; }
        public int PedidoComImpostoId { get; private set; }
        public int ClienteId { get; private set; }
        public int PedidoId { get; private set; }
        public decimal Imposto { get; private set; }
        public List<PedidoItemDTO> Itens { get; private set; }
        public EPedidoTributadoStatus Status { get; set; }
    }
}