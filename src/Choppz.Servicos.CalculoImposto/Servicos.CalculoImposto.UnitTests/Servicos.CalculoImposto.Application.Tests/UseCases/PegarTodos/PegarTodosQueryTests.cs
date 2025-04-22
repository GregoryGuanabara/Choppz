using Bogus;
using FluentAssertions;
using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.Queries.PegarTodos;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Application.Tests.UseCases.PegarTodos
{
    public class PegarTodosQueryTests
    {
        private readonly Faker _faker;

        public PegarTodosQueryTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void Construtor_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var status = EPedidoTributadoStatus.Criado;
            var pagina = _faker.Random.Int(1, 10);
            var itensPorPagina = _faker.Random.Int(1, 50);

            // Act
            var query = new PegarTodosQuery(status, pagina, itensPorPagina);

            // Assert
            query.Status.Should().Be(status);
            query.Pagina.Should().Be(pagina);
            query.ItensPorPagina.Should().Be(itensPorPagina);
        }

        [Fact]
        public void Construtor_DeveAceitarStatusNulo()
        {
            // Arrange
            var pagina = _faker.Random.Int(1, 10);
            var itensPorPagina = _faker.Random.Int(1, 50);

            // Act
            var query = new PegarTodosQuery(null, pagina, itensPorPagina);

            // Assert
            query.Status.Should().BeNull();
            query.Pagina.Should().Be(pagina);
            query.ItensPorPagina.Should().Be(itensPorPagina);
        }

        [Fact]
        public void Propriedades_DevemSerModificaveis()
        {
            // Arrange
            var query = new PegarTodosQuery(null, 1, 10);
            var novoStatus = EPedidoTributadoStatus.Cancelado;
            var novaPagina = _faker.Random.Int(2, 5);
            var novosItensPorPagina = _faker.Random.Int(20, 30);

            // Act
            query.Status = novoStatus;
            query.Pagina = novaPagina;
            query.ItensPorPagina = novosItensPorPagina;

            // Assert
            query.Status.Should().Be(novoStatus);
            query.Pagina.Should().Be(novaPagina);
            query.ItensPorPagina.Should().Be(novosItensPorPagina);
        }

        [Fact]
        public void Record_DeveTerIgualdadePorValor()
        {
            // Arrange
            var status = EPedidoTributadoStatus.Criado;
            var pagina = 1;
            var itensPorPagina = 10;
            var query1 = new PegarTodosQuery(status, pagina, itensPorPagina);
            var query2 = new PegarTodosQuery(status, pagina, itensPorPagina);

            // Act & Assert
            query1.Should().Be(query2);
            query1.GetHashCode().Should().Be(query2.GetHashCode());
        }

        [Fact]
        public void Record_NaoDeveSerIgual_QuandoPropriedadesDiferem()
        {
            // Arrange
            var query1 = new PegarTodosQuery(EPedidoTributadoStatus.Criado, 1, 10);
            var query2 = new PegarTodosQuery(EPedidoTributadoStatus.Cancelado, 1, 10);

            // Act & Assert
            query1.Should().NotBe(query2);
        }

        [Fact]
        public void DeveImplementarIRequest()
        {
            // Arrange
            var query = new PegarTodosQuery(null, 1, 10);

            // Act & Assert
            query.Should().BeAssignableTo<IRequest<RespostaPadronizadaModel>>();
        }
    }
}