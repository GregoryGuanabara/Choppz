using Bogus;
using FluentAssertions;
using MediatR;
using NSubstitute;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.Queries.PegarTodos;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Core.Tests.Builders;
using Xunit;

namespace Servicos.CalculoImposto.Application.Tests.Queries.PegarTodos
{
    public class PegarTodosHandlerTests
    {
        private readonly PegarTodosHandler _handler;
        private readonly IPedidoTributadoRepository _pedidoTributadoRepository;
        private readonly Faker _faker;

        public PegarTodosHandlerTests()
        {
            _pedidoTributadoRepository = Substitute.For<IPedidoTributadoRepository>();
            _handler = new PegarTodosHandler(_pedidoTributadoRepository);
            _faker = new Faker();
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoComDadosPaginados()
        {
            // Arrange
            var status = EPedidoTributadoStatus.Criado;
            var pagina = 1;
            var itensPorPagina = 10;
            var request = new PegarTodosQuery(status, pagina, itensPorPagina);

            var pedidos = new List<PedidoTributado>
            {
                new PedidoTributado(_faker.Random.Int(1,10), _faker.Random.Int(1, 10), _faker.Random.Decimal(), PedidoItemBuilder.CriarListaValida()),
                new PedidoTributado(_faker.Random.Int(1,10), _faker.Random.Int(1, 10), _faker.Random.Decimal(), PedidoItemBuilder.CriarListaValida())
            };

            var resultadoPaginado = new ResultadoPaginadoDTO<PedidoTributado>(pedidos, 2, 1, 1);

            _pedidoTributadoRepository.PegarTodosAsNoTracking(status, pagina, itensPorPagina)
                .Returns(resultadoPaginado);

            // Act
            var resultado = await _handler.Handle(request, CancellationToken.None);

            // Assert
            resultado.Sucesso.Should().BeTrue();
            resultado.Data.Should().BeOfType<ResultadoPaginadoModel<PedidoTributado>>();

            var dadosPaginados = resultado.Data as ResultadoPaginadoModel<PedidoTributado>;
            dadosPaginados.Should().NotBeNull(); 
            dadosPaginados.Itens.Should().HaveCount(2);
            dadosPaginados.TotalDeItens.Should().Be(2);
            dadosPaginados.TotaldePaginas.Should().Be(1);
            dadosPaginados.PaginaAtual.Should().Be(1);

            await _pedidoTributadoRepository.Received(1)
                .PegarTodosAsNoTracking(status, pagina, itensPorPagina);
        }

        [Fact]
        public async Task Handle_DeveRetornarSucessoComListaVazia_QuandoNaoHouverResultados()
        {
            // Arrange
            var request = new PegarTodosQuery(null, 1, 10);
            var resultadoPaginado = new ResultadoPaginadoDTO<PedidoTributado>(
                new List<PedidoTributado>(), 0, 0, 1);

            _pedidoTributadoRepository.PegarTodosAsNoTracking(null, 1, 10)
                .Returns(resultadoPaginado);

            // Act
            var resultado = await _handler.Handle(request, CancellationToken.None);

            // Assert
            resultado.Sucesso.Should().BeTrue();
            var dadosPaginados = resultado.Data as ResultadoPaginadoModel<PedidoTributado>;
            dadosPaginados.Should().NotBeNull();
            dadosPaginados.Itens.Should().BeEmpty();
            dadosPaginados.TotalDeItens.Should().Be(0);
        }

        [Fact]
        public async Task Handle_DevePassarParametrosCorretosParaORepositorio()
        {
            // Arrange
            var status = EPedidoTributadoStatus.Criado;
            var pagina = 2;
            var itensPorPagina = 5;
            var request = new PegarTodosQuery(status, pagina, itensPorPagina);

            _pedidoTributadoRepository.PegarTodosAsNoTracking(Arg.Any<EPedidoTributadoStatus?>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(new ResultadoPaginadoDTO<PedidoTributado>(new List<PedidoTributado>(), 0, 0, 1));

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            await _pedidoTributadoRepository.Received(1)
                .PegarTodosAsNoTracking(status, pagina, itensPorPagina);
        }

        [Fact]
        public async Task Handle_DevePassarStatusNulo_QuandoNaoEspecificado()
        {
            // Arrange
            var request = new PegarTodosQuery(null, 1, 10);

            _pedidoTributadoRepository.PegarTodosAsNoTracking(Arg.Any<EPedidoTributadoStatus?>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(new ResultadoPaginadoDTO<PedidoTributado>(new List<PedidoTributado>(), 0, 0, 1));

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            await _pedidoTributadoRepository.Received(1)
                .PegarTodosAsNoTracking(null, 1, 10);
        }

        [Fact]
        public async Task Handle_DeveConverterCorretamenteParaRespostaPadronizada()
        {
            // Arrange
            var request = new PegarTodosQuery(null, 1, 10);
            var pedidos = new List<PedidoTributado>
            {
                new PedidoTributado(_faker.Random.Int(1,10), _faker.Random.Int(1, 10), _faker.Random.Decimal(), PedidoItemBuilder.CriarListaValida()),
            };

            var resultadoPaginado = new ResultadoPaginadoDTO<PedidoTributado>(pedidos, 1, 1, 1);

            _pedidoTributadoRepository.PegarTodosAsNoTracking(null, 1, 10)
                .Returns(resultadoPaginado);

            // Act
            var resultado = await _handler.Handle(request, CancellationToken.None);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Sucesso.Should().BeTrue();
            resultado.Data.Should().BeOfType<ResultadoPaginadoModel<PedidoTributado>>();
        }
    }
}