using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.Infra.Persistence.Repositories
{
    public class PedidoTributadoRepository : IPedidoTributadoRepository
    {
        public Task<bool> ExistePedidoAsync(int pedidoId)
        {
            throw new NotImplementedException();
        }

        public Task InserirAsync(PedidoTributado pedido)
        {
            throw new NotImplementedException();
        }

        public Task<PedidoTributado?> PegarPeloPedidoIdAsNoTrackingAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultadoPaginadoDTO<PedidoTributado>> PegarTodosAsNoTracking(EPedidoTributadoStatus? status, int pagina, int itensPorPagina)
        {
            throw new NotImplementedException();
        }
    }
}
