using FluentValidation;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;

namespace Servicos.CalculoImposto.Core.Validators
{
    public class PedidoItemValidator : AbstractValidator<PedidoItem>
    {
        public PedidoItemValidator()
        {
            RuleFor(item => item.ProdutoId)
                .GreaterThan(0).WithMessage("ProdutoId deve ser maior que 0");

            RuleFor(item => item.Valor)
                .GreaterThanOrEqualTo(0).WithMessage("Valor unitário não pode ser menor que 0");

            RuleFor(item => item.Quantidade)
                .GreaterThan(0).WithMessage("Quantidade deve ser maior que 0");
        }
    }
}