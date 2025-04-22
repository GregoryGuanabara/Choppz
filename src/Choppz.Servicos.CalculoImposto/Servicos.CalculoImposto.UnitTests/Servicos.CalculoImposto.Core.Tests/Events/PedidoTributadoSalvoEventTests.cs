using Bogus;
using FluentAssertions;
using NSubstitute.ExceptionExtensions;
using Servicos.CalculoImposto.Core.DTOs;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Core.Events;
using Servicos.CalculoImposto.UnitTests.Servicos.CalculoImposto.Core.Tests.Builders;
using System;
using System.Collections.Generic;
using Xunit;

namespace Servicos.CalculoImposto.Core.Tests.Events
{
    public class PedidoTributadoSalvoEventTests
    {
        private readonly Faker _faker = new Faker();

        [Fact]
        public void Constructor_DeveInicializarCorretamente()
        {
            // Arrange
            var pedidoComImpostoId = _faker.Random.Int(1, 1000);
            var pedidoId = _faker.Random.Int(1, 1000);
            var clienteId = _faker.Random.Int(1, 1000);
            var imposto = _faker.Random.Decimal(1, 100);
            var itens = PedidoItemDTOBuilders.CriarItensValidos();
            var status = EPedidoTributadoStatus.Criado;

            // Act
            var @event = new PedidoTributadoSalvoEvent(
                pedidoComImpostoId,
                pedidoId,
                clienteId,
                imposto,
                itens,
                status);

            // Assert
            @event.PedidoComImpostoId.Should().Be(pedidoComImpostoId);
            @event.PedidoId.Should().Be(pedidoId);
            @event.ClienteId.Should().Be(clienteId);
            @event.Imposto.Should().Be(imposto);
            @event.Itens.Should().BeEquivalentTo(itens);
            @event.Status.Should().Be(status);
            @event.OcorridoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_ComPedidoComImpostoIdInvalido_DeveLancarException(int idInvalido)
        {
            // Arrange
            var itens = PedidoItemDTOBuilders.CriarItensValidos();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new PedidoTributadoSalvoEvent(
                    idInvalido,
                    1,
                    1,
                    10m,
                    itens,
                    EPedidoTributadoStatus.Criado))
                .ParamName.Should().Be("pedidoComImpostoId");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_ComPedidoIdInvalido_DeveLancarException(int idInvalido)
        {
            // Arrange
            var itens = PedidoItemDTOBuilders.CriarItensValidos();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new PedidoTributadoSalvoEvent(
                    1,
                    idInvalido,
                    1,
                    10m,
                    itens,
                    EPedidoTributadoStatus.Criado))
                .ParamName.Should().Be("pedidoId");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_ComClienteIdInvalido_DeveLancarException(int idInvalido)
        {
            // Arrange
            var itens = PedidoItemDTOBuilders.CriarItensValidos();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new PedidoTributadoSalvoEvent(
                    1,
                    1,
                    idInvalido,
                    10m,
                    itens,
                    EPedidoTributadoStatus.Criado))
                .ParamName.Should().Be("clienteId");
        }

        [Fact]
        public void Constructor_ComImpostoNegativo_DeveLancarException()
        {
            // Arrange
            var itens = PedidoItemDTOBuilders.CriarItensValidos();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new PedidoTributadoSalvoEvent(
                    1,
                    1,
                    1,
                    -1m,
                    itens,
                    EPedidoTributadoStatus.Criado))
                .ParamName.Should().Be("imposto");
        }

        [Fact]
        public void Constructor_ComItensNulos_DeveLancarException()
        {
            // Arrange
            var itens = (List<PedidoItemDTO>?)null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new PedidoTributadoSalvoEvent(
                    1,
                    1,
                    1,
                    10m,
                    itens!,
                    EPedidoTributadoStatus.Criado))
                .ParamName.Should().Be("itens");
        }

        [Fact]
        public void Constructor_ComListaItensVazia_DeveLancarException()
        {
            // Arrange
            var itensVazios = new List<PedidoItemDTO>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                new PedidoTributadoSalvoEvent(
                    1,
                    1,
                    1,
                    10m,
                    itensVazios,
                    EPedidoTributadoStatus.Criado))
                .ParamName.Should().Be("itens");
        }


        [Fact]
        public void Propriedades_DevemTerAcessibilidadeCorreta()
        {
            // Arrange
            var @event = new PedidoTributadoSalvoEvent(
                1, 1, 1, 10m, PedidoItemDTOBuilders.CriarItensValidos(), EPedidoTributadoStatus.Criado);

            // Assert
            @event.GetType().GetProperty(nameof(PedidoTributadoSalvoEvent.PedidoComImpostoId))!.SetMethod!.IsPrivate.Should().BeTrue();
            @event.GetType().GetProperty(nameof(PedidoTributadoSalvoEvent.PedidoId))!.SetMethod!.IsPrivate.Should().BeTrue();
            @event.GetType().GetProperty(nameof(PedidoTributadoSalvoEvent.ClienteId))!.SetMethod!.IsPrivate.Should().BeTrue();
            @event.GetType().GetProperty(nameof(PedidoTributadoSalvoEvent.Imposto))!.SetMethod!.IsPrivate.Should().BeTrue();
            @event.GetType().GetProperty(nameof(PedidoTributadoSalvoEvent.Itens))!.SetMethod!.IsPrivate.Should().BeTrue();
            @event.GetType().GetProperty(nameof(PedidoTributadoSalvoEvent.Status))!.SetMethod!.IsPrivate.Should().BeTrue();
            @event.GetType().GetProperty(nameof(PedidoTributadoSalvoEvent.OcorridoEm))!.SetMethod!.IsPrivate.Should().BeTrue();
        }
    }
}