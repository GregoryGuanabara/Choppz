using Bogus;
using FluentAssertions;
using NSubstitute;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.UseCases.CalcularImposto;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.Services;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Events;

namespace Servicos.CalculoImposto.Application.Tests.UseCases.CalcularImpostos
{
    public class CalcularImpostoHandlerTests
    {
        private readonly CalcularImpostoHandler _handler;
        private readonly IPedidoTributadoRepository _pedidoTributadoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;
        private readonly IImpostoService _impostoService;
        private readonly Faker _faker = new Faker();

        public CalcularImpostoHandlerTests()
        {
            _pedidoTributadoRepository = Substitute.For<IPedidoTributadoRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _cacheService = Substitute.For<ICacheService>();
            _impostoService = Substitute.For<IImpostoService>();

            _handler = new CalcularImpostoHandler(_pedidoTributadoRepository,
                                                  _unitOfWork,
                                                  _cacheService,
                                                  _impostoService);
        }

        [Fact]
        public async Task Handle_QuandoPedidoExisteNoCache_DeveRetornarErro()
        {
            // Arrange
            var request = new CalcularImpostoCommand(1, 1, new List<PedidoItemModel>());
            _cacheService.Existe(Arg.Any<string>()).Returns(true);
            _pedidoTributadoRepository.ExistePedidoAsync(Arg.Any<int>()).Returns(true);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Sucesso.Should().BeFalse();
            result.Data.Should().BeOfType<ResultadoDeErrosModel>()
                .Which.Erros.Should().OnlyContain(x => x.Mensagem == "O Pedido já existe.");
            await _pedidoTributadoRepository.DidNotReceive().InserirAsync(Arg.Any<PedidoTributado>());
        }

        [Fact]
        public async Task Handle_QuandoPedidoNaoExiste_DeveCalcularEInserir()
        {
            // Arrange
            var request = new CalcularImpostoCommand(1, 1, new List<PedidoItemModel>
            {
                new PedidoItemModel(1, 2, 100m)
            });

            _cacheService.Existe(Arg.Any<string>()).Returns(false);
            _impostoService.CalcularImposto(Arg.Any<decimal>()).Returns(20m);
            _pedidoTributadoRepository.ExistePedidoAsync(Arg.Any<int>()).Returns(false);

            PedidoTributado? pedidoCriadoNoHandler = null;

            await _pedidoTributadoRepository.InserirAsync(Arg.Do<PedidoTributado>(x =>
            {
                pedidoCriadoNoHandler = x;
                x.GetType().BaseType?.GetProperty("Id")?.SetValue(x, _faker.Random.Int(1, 10));
            }));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Sucesso.Should().BeTrue();
            await _pedidoTributadoRepository.Received(1).InserirAsync(Arg.Any<PedidoTributado>());
            await _unitOfWork.Received(1).SaveChangesAsync();
            _impostoService.Received(1).CalcularImposto(200m);
        }

        [Fact]
        public async Task Handle_DeveDispararEventoPedidoSalvo()
        {
            // Arrange
            var request = new CalcularImpostoCommand(1, 1, new List<PedidoItemModel>
            {
                new PedidoItemModel(1, 1, 100m)
            });

            _cacheService.Existe(Arg.Any<string>()).Returns(false);
            _pedidoTributadoRepository.ExistePedidoAsync(Arg.Any<int>()).Returns(false);
            _impostoService.CalcularImposto(Arg.Any<decimal>()).Returns(10m);

            PedidoTributado? pedidoCriadoNoHandler = null;

            await _pedidoTributadoRepository.InserirAsync(Arg.Do<PedidoTributado>(x =>
            {
                pedidoCriadoNoHandler = x;
                x.GetType().BaseType?.GetProperty("Id")?.SetValue(x, _faker.Random.Int(1, 10));
            }));

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            pedidoCriadoNoHandler.Should().NotBeNull();
            pedidoCriadoNoHandler.Events.Should().ContainSingle()
                .Which.Should().BeOfType<PedidoTributadoSalvoEvent>();
        }

        [Fact]
        public async Task Handle_DeveUsarChaveCorretaNoCache()
        {
            // Arrange
            var pedidoId = 123;

            // Arrange
            var request = new CalcularImpostoCommand(pedidoId, 1, new List<PedidoItemModel>
            {
                new PedidoItemModel(1, 1, 100m)
            });

            string? cacheKeyUsed = null;
            _cacheService.Existe(Arg.Do<string>(x => cacheKeyUsed = x)).Returns(false);

            PedidoTributado? pedidoCriadoNoHandler = null;

            await _pedidoTributadoRepository.InserirAsync(Arg.Do<PedidoTributado>(x =>
            {
                pedidoCriadoNoHandler = x;
                x.GetType().BaseType?.GetProperty("Id")?.SetValue(x, pedidoId);
            }));

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            cacheKeyUsed.Should().Be($"Pedido-{pedidoId}");
        }
    }
}