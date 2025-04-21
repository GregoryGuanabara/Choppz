using Bogus;
using Servicos.CalculoImposto.Application.Models;

namespace Servicos.CalculoImposto.UnitTests.Servicos.CalculoImposto.Application.Tests.Builders
{
    public class PedidoItemModelBuilder
    {
        private readonly Faker _faker = new Faker();

        private int _produtoId;
        private int _quantidade;
        private decimal _valorUnitario;

        public PedidoItemModelBuilder()
        {
            _produtoId = _faker.Random.Int(1, 1000);
            _quantidade = _faker.Random.Int(1, 10);
            _valorUnitario = _faker.Random.Decimal(10, 500);
        }

        public PedidoItemModel Build()
        {
            return new PedidoItemModel(_produtoId, _quantidade, _valorUnitario);
        }

        public static List<PedidoItemModel> CriarItensValidos(int quantidade = 2)
        {
            var builder = new PedidoItemModelBuilder();
            var lista = new List<PedidoItemModel>();

            for (int i = 0; i < quantidade; i++)
                lista.Add(builder.Build());

            return lista;
        }
    }
}