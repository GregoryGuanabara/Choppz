using Servicos.CalculoImposto.Core.BaseEntities;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Entities.PedidoTributado
{
    public sealed class PedidoTributado : AggregateRoot
    {
        private readonly List<PedidoItem> _itens;

        private PedidoTributado()
        {
            _itens = new List<PedidoItem>();
        }

        public PedidoTributado(int pedidoId, int clienteId, decimal imposto, EPedidoTributadoStatus status, List<PedidoItem> itens)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Imposto = imposto;
            Status = status;
            _itens = itens;
        }

        public int PedidoId { get; private set; }
        public int ClienteId { get; private set; }
        public decimal Imposto { get; private set; }
        public EPedidoTributadoStatus Status { get; private set; }
        public IReadOnlyList<PedidoItem> Itens => _itens;

        public void Cancelar()
        {
            if (Status == EPedidoTributadoStatus.Cancelado)
                return;

            Status = EPedidoTributadoStatus.Cancelado;
            AtualizarModificadoEm();
        }
    }
}