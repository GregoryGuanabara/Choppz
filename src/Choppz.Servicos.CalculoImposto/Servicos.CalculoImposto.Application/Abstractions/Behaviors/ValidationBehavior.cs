using FluentValidation;
using MediatR;
using Servicos.CalculoImposto.Application.Models;

namespace Servicos.CalculoImposto.Application.Abstractions.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators.Select(v => v.Validate(context))
                                      .SelectMany(result => result.Errors)
                                      .Where(f => f != null)
                                      .ToList();

            if (failures.Any())
            {
                var mensagensErro = failures.Select(f => new ErroModel(f.PropertyName, f.ErrorMessage)).ToList();

                var errorResponse = (TResponse)(object)RespostaPadronizadaModel.ComErros(mensagensErro);

                return errorResponse;
            }

            return await next();
        }
    }
}