using Servicos.CalculoImposto.Core.Abstractions.DomainException;
using Servicos.CalculoImposto.Core.Abstractions.Validators;
using Servicos.CalculoImposto.Core.BaseEntities;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Core.Validators;

namespace Servicos.CalculoImposto.Core.Entities.PedidoTributado
{
    public sealed class PedidoTributado : AggregateRoot, IValidatable
    {
        private readonly List<PedidoItem> _itens;

        private PedidoTributado()
        {
            _itens = new List<PedidoItem>();
        }

        public PedidoTributado(int pedidoId, int clienteId, decimal imposto, List<PedidoItem> itens)
        {
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Imposto = imposto;
            Status = EPedidoTributadoStatus.Criado;
            _itens = itens;
            Validate();
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

        public void Validate()
        {
            var validator = new PedidoTributadoValidator();
            var result = validator.Validate(this);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    throw new DomainException(error.ErrorMessage);
            }
        }
    }
}