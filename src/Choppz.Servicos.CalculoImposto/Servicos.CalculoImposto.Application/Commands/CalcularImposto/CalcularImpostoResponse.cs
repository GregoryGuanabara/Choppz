using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Application.Commands.CalcularImposto
{
    public sealed record CalcularImpostoResponse
    {
        public CalcularImpostoResponse(int id, EPedidoTributadoStatus status)
        {
            Id = id;
            Status = status.ToString();
        }

        public int Id { get; set; }
        public string Status { get; set; }
    }
}