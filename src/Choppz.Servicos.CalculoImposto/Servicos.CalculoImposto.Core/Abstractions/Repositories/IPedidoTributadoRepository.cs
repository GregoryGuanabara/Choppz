using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Abstractions.Repositories
{
    public interface IPedidoTributadoRepository
    {
        Task<PedidoTributado?> PegarPeloPedidoIdAsNoTrackingAsync(int id);

        Task InserirAsync(PedidoTributado pedido);

        Task<bool> ExistePedidoAsync(int pedidoId);

        Task<ResultadoPaginadoDTO<PedidoTributado>> PegarTodosAsNoTracking(EPedidoTributadoStatus? status, int pagina, int itensPorPagina);
    }
}