namespace Servicos.CalculoImposto.Core.Entities.PedidoTributado
{
    public sealed class PedidoItem
    {
        private PedidoItem()
        { }

        public PedidoItem(int produtoId, int quantidade, decimal valor)
        {
            ProdutoId = produtoId;
            Quantidade = quantidade;
            Valor = valor;
        }

        public int ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Valor { get; private set; }
    }
}