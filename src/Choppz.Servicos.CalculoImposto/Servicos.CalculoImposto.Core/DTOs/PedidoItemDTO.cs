namespace Servicos.CalculoImposto.Core.DTOs
{
    public record PedidoItemDTO
    {
        public PedidoItemDTO(int produtoId, int quantidade, decimal valor)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Valor = valor;
        }

        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
    }
}