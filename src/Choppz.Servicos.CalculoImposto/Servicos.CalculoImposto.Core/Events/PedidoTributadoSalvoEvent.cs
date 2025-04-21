using Servicos.CalculoImposto.Core.BaseEntities;
using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Events
{
    public sealed record PedidoTributadoSalvoEvent : IDomainEvent
    {
        public PedidoTributadoSalvoEvent(int pedidoComImpostoId, int pedidoId, int clienteId, decimal imposto, List<PedidoItemDTO> itens, EPedidoTributadoStatus status)
        {
            if (pedidoComImpostoId <= 0)
                throw new ArgumentException("ID do pedido com imposto deve ser maior que zero", nameof(pedidoComImpostoId));

            if (pedidoId <= 0)
                throw new ArgumentException("ID do pedido deve ser maior que zero", nameof(pedidoId));

            if (clienteId <= 0)
                throw new ArgumentException("ID do cliente deve ser maior que zero", nameof(clienteId));

            if (imposto < 0)
                throw new ArgumentException("Imposto deve ser igual ou maior que zero", nameof(imposto));

            if (itens == null)
                throw new ArgumentNullException(nameof(itens), "Lista de itens não pode ser nula");

            if (!itens.Any())
                throw new ArgumentException("Pedido deve conter pelo menos um item", nameof(itens));

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
        public EPedidoTributadoStatus Status { get; private set; }
    }
}