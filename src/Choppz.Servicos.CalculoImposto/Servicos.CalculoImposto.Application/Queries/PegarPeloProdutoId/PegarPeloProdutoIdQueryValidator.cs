using FluentValidation;

namespace Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId
{
    public class PegarPeloProdutoIdQueryValidator : AbstractValidator<PegarPeloPedidoIdQuery>
    {
        public PegarPeloProdutoIdQueryValidator()
        {
            RuleFor(x => x.PedidoId)
                    .NotEmpty().WithMessage("O ID do pedido não pode ser vazio.")
                    .GreaterThan(0).WithMessage("O ID do pedido deve ser um valor valido.");
        }
    }
}