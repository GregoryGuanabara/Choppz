namespace Servicos.CalculoImposto.Core.Entities.PedidoTributado
{
    public sealed class PedidoItem
    {
        private PedidoItem()
        { }

        public PedidoItem(int produtoId, int quantidade, decimal valor)
        {
            if (produtoId <= 0)
                throw new ArgumentException("ProdutoId deve ser maior que 0");

            ProdutoId = produtoId;

            if (valor < 0)
                throw new ArgumentException("Valor unitário não pode ser menor que 0");

            Valor = valor;

            if (quantidade <= 0)
                throw new ArgumentException("Quantidade deve ser maior que 0");

            Quantidade = quantidade;
            
        }

        public int ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal Valor { get; private set; }
    }
}