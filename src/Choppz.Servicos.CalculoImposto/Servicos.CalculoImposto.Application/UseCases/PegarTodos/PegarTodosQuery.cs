using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Application.Queries.PegarTodos
{
    public sealed record PegarTodosQuery : IRequest<RespostaPadronizadaModel>
    {
        public PegarTodosQuery(EPedidoTributadoStatus? status, int pagina, int itensPorPagina)
        {
            Status = status;
            Pagina = pagina;
            ItensPorPagina = itensPorPagina;
        }

        public EPedidoTributadoStatus? Status { get; set; }
        public int Pagina { get; set; }
        public int ItensPorPagina { get; set; }
    }
}