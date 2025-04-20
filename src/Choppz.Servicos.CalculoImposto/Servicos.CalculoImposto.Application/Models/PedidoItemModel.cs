using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;

namespace Servicos.CalculoImposto.Application.Models
{
    public sealed record PedidoItemModel
    {
        public PedidoItemModel(int produtoId, int quantidade, decimal valor)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Valor = valor;
        }

        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }

        public static List<PedidoItemModel> ConverterParaModels(List<PedidoItemDTO> pedidoItens)
        {
            var response = new List<PedidoItemModel>();

            foreach (var item in pedidoItens)
                response.Add(new PedidoItemModel(item.ProdutoId, item.Quantidade, item.Valor));

            return response;
        }

        public static List<PedidoItemModel> ConverterParaModels(IReadOnlyCollection<PedidoItem> pedidoItens)
        {
            var response = new List<PedidoItemModel>();

            foreach (var item in pedidoItens)
                response.Add(new PedidoItemModel(item.ProdutoId, item.Quantidade, item.Valor));

            return response;
        }
    }
}