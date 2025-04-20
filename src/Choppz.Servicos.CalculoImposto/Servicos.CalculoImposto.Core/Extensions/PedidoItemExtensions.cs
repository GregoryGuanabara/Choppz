using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;

namespace Servicos.CalculoImposto.Core.Extensions
{
    public static class PedidoItemExtensions
    {
        public static List<PedidoItemDTO> ToDTOs(this List<PedidoItem> pedidoItens)
        {
            var response = new List<PedidoItemDTO>();

            foreach (var item in pedidoItens)
                response.Add(new PedidoItemDTO(item.ProdutoId, item.Quantidade, item.Valor));

            return response;
        }
    }
}