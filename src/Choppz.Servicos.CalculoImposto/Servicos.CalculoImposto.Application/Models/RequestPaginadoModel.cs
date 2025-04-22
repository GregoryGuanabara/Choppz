using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Application.Models
{
    public sealed record RequestPaginadoModel
    {
        public int Pagina { get; set; } = 1;
        public int ItensPorPagina { get; set; } = 10;
        public EPedidoTributadoStatus? Status { get; set; }
    }
}