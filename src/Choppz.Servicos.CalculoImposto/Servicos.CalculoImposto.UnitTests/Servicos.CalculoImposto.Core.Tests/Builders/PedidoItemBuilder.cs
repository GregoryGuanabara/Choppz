using Bogus;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;

namespace Servicos.CalculoImposto.Core.Tests.Builders
{
    public class PedidoItemBuilder
    {
        private readonly Faker _faker = new Faker();
        private int _produtoId;
        private int _quantidade;
        private decimal _valorUnitario;

        public PedidoItemBuilder()
        {
            _produtoId = _faker.Random.Int(1, 1000);
            _quantidade = _faker.Random.Int(1, 10);
            _valorUnitario = _faker.Random.Decimal(10, 500);
        }

        public PedidoItemBuilder ComProdutoId(int produtoId)
        {
            _produtoId = produtoId;
            return this;
        }

        public PedidoItemBuilder ComQuantidade(int quantidade)
        {
            _quantidade = quantidade;
            return this;
        }

        public PedidoItemBuilder ComValorUnitario(decimal valor)
        {
            _valorUnitario = valor;
            return this;
        }

        public PedidoItem Build()
        {
            return new PedidoItem(_produtoId, _quantidade, _valorUnitario);
        }

        public static List<PedidoItem> CriarListaValida(int quantidade = 2)
        {
            var builder = new PedidoItemBuilder();
            var lista = new List<PedidoItem>();

            for (int i = 0; i < quantidade; i++)
            {
                lista.Add(builder.Build());
            }

            return lista;
        }
    }
}