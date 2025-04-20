using FluentValidation;

namespace Servicos.CalculoImposto.Application.Commands.CalcularImposto
{
    public class CalcularImpostoCommandValidator : AbstractValidator<CalcularImpostoCommand>
    {
        public CalcularImpostoCommandValidator()
        {
            RuleFor(x => x.PedidoId)
                .NotEmpty().WithMessage("O ID do pedido não pode ser vazio.")
                .NotEqual(0).WithMessage("O ID do pedido deve ser um valor valido.");

            RuleFor(x => x.ClienteId)
                .NotEmpty().WithMessage("O ID do cliente não pode ser vazio.")
                .NotEqual(0).WithMessage("O ID do cliente deve ser um valor valido.");

            RuleFor(x => x.Itens)
                .NotEmpty()
                .WithMessage("A lista de itens não pode ser vazia.");
        }
    }
}