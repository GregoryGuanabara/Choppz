using FluentValidation;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Application.Queries.PegarTodos
{
    public sealed class PegarTodosQueryValidator : AbstractValidator<PegarTodosQuery>
    {
        public PegarTodosQueryValidator()
        {
            RuleFor(x => x.Status)
                .Must(status => status == null || Enum.IsDefined(typeof(EPedidoTributadoStatus), status))
                .WithMessage("O status do pedido é inválido.");

            RuleFor(x => x.Pagina)
                .GreaterThan(0)
                .WithMessage("O valor de pagina não é valido.");

            RuleFor(x => x.ItensPorPagina)
                .GreaterThanOrEqualTo(1)
                .WithMessage("O valor de itens por pagina não é valido. Mínimo permitido = 1.")
                .LessThanOrEqualTo(100)
                 .WithMessage("O valor de itens por pagina não é valido. Máximo permitido = 100.");
        }
    }
}