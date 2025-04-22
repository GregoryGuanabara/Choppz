using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;

namespace Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId
{
    public class PegarPeloPedidoIdQueryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ICacheService _cacheService;

        public PegarPeloPedidoIdQueryBehavior(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is not PegarPeloPedidoIdQuery pedidoRequest)
                return await next();

            string cacheKey = $"Pedido-{pedidoRequest.PedidoId}";

            var cachedResponse = _cacheService.Pegar<TResponse>(cacheKey);

            if (cachedResponse is not null)
                return cachedResponse;

            var response = await next();

            if (response is RespostaPadronizadaModel result && result.Sucesso)
                _cacheService.Inserir(cacheKey, response, TimeSpan.FromMinutes(10));

            return response;
        }
    }
}