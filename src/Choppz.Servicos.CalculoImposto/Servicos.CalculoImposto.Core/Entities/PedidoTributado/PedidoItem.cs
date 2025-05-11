using Servicos.CalculoImposto.Core.Abstractions.DomainException;
using Servicos.CalculoImposto.Core.Abstractions.Validators;
using Servicos.CalculoImposto.Core.BaseEntities;
using Servicos.CalculoImposto.Core.Validators;

namespace Servicos.CalculoImposto.Core.Entities.PedidoTributado
{
    public sealed class PedidoItem : IEntityBase, IValidatable
    {
        private PedidoItem()
        { }

        public PedidoItem(int produtoId, int quantidade, decimal valor)
        {
            ProdutoId = produtoId;
            Valor = valor;
            Quantidade = quantidade;
            Validate();
        }

        public int Id { get; private set; }
        public int ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Valor { get; private set; }

        public void Validate()
        {
            var validator = new PedidoItemValidator();
            var result = validator.Validate(this);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    throw new DomainException(error.ErrorMessage);
            }
        }
    }
}