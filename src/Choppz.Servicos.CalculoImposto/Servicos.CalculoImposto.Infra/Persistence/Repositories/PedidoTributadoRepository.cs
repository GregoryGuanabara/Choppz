using Microsoft.EntityFrameworkCore;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Infra.Persistence.Repositories
{
    public class PedidoTributadoRepository : IPedidoTributadoRepository
    {
        private readonly DbSet<PedidoTributado> _pedidosTributados;

        public PedidoTributadoRepository(ApplicationDbContext context)
        {
            _pedidosTributados = context.Set<PedidoTributado>();
        }

        public async Task<bool> ExistePedidoAsync(int pedidoId)
        {
            return await _pedidosTributados.AnyAsync(p => p.PedidoId == pedidoId);
        }

        public async Task InserirAsync(PedidoTributado pedido)
        {
            await _pedidosTributados.AddAsync(pedido);
        }

        public async Task<PedidoTributado?> PegarPeloPedidoIdAsNoTrackingAsync(int pedidoId)
        {
            return await _pedidosTributados.AsNoTracking().SingleOrDefaultAsync(p => p.PedidoId == pedidoId);
        }

        public async Task<ResultadoPaginadoDTO<PedidoTributado>> PegarTodosAsNoTracking(EPedidoTributadoStatus? status, int pagina, int itensPorPagina)
        {
            var query = _pedidosTributados.AsNoTracking();

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            int totalItens = await query.CountAsync();

            var itens = await query.OrderBy(p => p.PedidoId)
                                   .Skip((pagina - 1) * itensPorPagina)
                                   .Take(itensPorPagina)
                                   .ToListAsync();

            return new ResultadoPaginadoDTO<PedidoTributado>(itens, totalItens, (int)Math.Ceiling(totalItens / (double)itensPorPagina), pagina);
        }
    }
}