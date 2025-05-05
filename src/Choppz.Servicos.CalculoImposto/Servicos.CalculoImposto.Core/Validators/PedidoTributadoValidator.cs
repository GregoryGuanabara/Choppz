using FluentValidation;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;

namespace Servicos.CalculoImposto.Core.Validators
{
    public class PedidoTributadoValidator : AbstractValidator<PedidoTributado>
    {
        public PedidoTributadoValidator()
        {
            RuleFor(pedido => pedido.PedidoId)
                .GreaterThan(0).WithMessage("ID do pedido inválido");

            RuleFor(pedido => pedido.ClienteId)
                .GreaterThan(0).WithMessage("ID do cliente inválido");

            RuleFor(pedido => pedido.Imposto)
                .GreaterThanOrEqualTo(0).WithMessage("Valor do imposto não pode ser negativo");

            RuleFor(pedido => pedido.Itens)
                .NotNull().WithMessage("Lista de itens não pode ser nula")
                .NotEmpty().WithMessage("Pedido deve conter pelo menos um item")
                .ForEach(item => item.SetValidator(new PedidoItemValidator()));

            RuleFor(pedido => pedido.Status)
                .IsInEnum().WithMessage("Status do pedido inválido");
        }
    }
}