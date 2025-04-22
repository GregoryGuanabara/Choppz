using Bogus;
using Servicos.CalculoImposto.Core.DTOs;

namespace Servicos.CalculoImposto.UnitTests.Servicos.CalculoImposto.Core.Tests.Builders
{
    public class PedidoItemDTOBuilders
    {
        private readonly Faker _faker = new Faker();
        private int _produtoId;
        private int _quantidade;
        private decimal _valorUnitario;

        public PedidoItemDTOBuilders()
        {
            _produtoId = _faker.Random.Int(1, 1000);
            _quantidade = _faker.Random.Int(1, 10);
            _valorUnitario = _faker.Random.Decimal(10, 500);
        }

        public PedidoItemDTOBuilders ComProdutoId(int produtoId)
        {
            _produtoId = produtoId;
            return this;
        }

        public PedidoItemDTOBuilders ComQuantidade(int quantidade)
        {
            _quantidade = quantidade;
            return this;
        }

        public PedidoItemDTOBuilders ComValorUnitario(decimal valor)
        {
            _valorUnitario = valor;
            return this;
        }

        public PedidoItemDTO Build()
        {
            return new PedidoItemDTO(_produtoId, _quantidade, _valorUnitario);
        }

        public static List<PedidoItemDTO> CriarItensValidos(int quantidade = 2)
        {
            var builder = new PedidoItemDTOBuilders();
            var lista = new List<PedidoItemDTO>();

            for (int i = 0; i < quantidade; i++)
                lista.Add(builder.Build());

            return lista;
        }
    }
}