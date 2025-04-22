using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;

namespace Servicos.CalculoImposto.Application.Queries.PegarTodos
{
    public sealed class PegarTodosHandler : IRequestHandler<PegarTodosQuery, RespostaPadronizadaModel>
    {
        private readonly IPedidoTributadoRepository _pedidoTributadoRepository;

        public PegarTodosHandler(IPedidoTributadoRepository pedidoTributadoRepository)
        {
            _pedidoTributadoRepository = pedidoTributadoRepository;
        }

        public async Task<RespostaPadronizadaModel> Handle(PegarTodosQuery request, CancellationToken cancellationToken)
        {
            var resultados = await _pedidoTributadoRepository.PegarTodosAsNoTracking(request.Status, request.Pagina, request.ItensPorPagina);

            var response = new ResultadoPaginadoModel<PedidoTributado>(resultados.Itens, resultados.TotalDeItens, resultados.TotaldePaginas, resultados.PaginaAtual);

            return RespostaPadronizadaModel.ComSucesso(response);
        }
    }
}