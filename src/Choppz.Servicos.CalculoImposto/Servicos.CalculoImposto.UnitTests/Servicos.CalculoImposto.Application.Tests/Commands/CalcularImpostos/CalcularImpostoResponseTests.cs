using Bogus;
using FluentAssertions;
using Servicos.CalculoImposto.Application.Commands.CalcularImposto;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Application.Tests.Commands.CalcularImposto
{
    public class CalcularImpostoResponseTests
    {
        private readonly Faker _faker;

        public CalcularImpostoResponseTests()
        {
            _faker = new Faker();
        }

        [Fact]
        public void Construtor_DeveInicializarPropriedadesCorretamente()
        {
            // Arrange
            var expectedId = _faker.Random.Int(1, 1000);
            var expectedStatus = EPedidoTributadoStatus.Criado;

            // Act
            var response = new CalcularImpostoResponse(expectedId, expectedStatus);

            // Assert
            response.Id.Should().Be(expectedId);
            response.Status.Should().Be(expectedStatus.ToString());
        }

        [Fact]
        public void Propriedades_DevemSerModificaveis()
        {
            // Arrange
            var initialId = _faker.Random.Int(1, 1000);
            var initialStatus = EPedidoTributadoStatus.Cancelado;
            var response = new CalcularImpostoResponse(initialId, initialStatus);

            var newId = _faker.Random.Int(1001, 2000);
            var newStatus = "CustomStatus";

            // Act
            response.Id = newId;
            response.Status = newStatus;

            // Assert
            response.Id.Should().Be(newId);
            response.Status.Should().Be(newStatus);
        }

        [Fact]
        public void Record_DeveTerIgualdadePorValor()
        {
            // Arrange
            var id = _faker.Random.Int(1, 100);
            var status = EPedidoTributadoStatus.Criado;

            // Act
            var response1 = new CalcularImpostoResponse(id, status);
            var response2 = new CalcularImpostoResponse(id, status);

            // Assert
            response1.Should().Be(response2);
            response1.GetHashCode().Should().Be(response2.GetHashCode());
        }

        [Fact]
        public void Record_NaoDeveSerIgual_QuandoPropriedadesDiferem()
        {
            // Arrange
            var id1 = _faker.Random.Int(1, 100);
            var id2 = id1 + 1;
            var status = EPedidoTributadoStatus.Criado;

            // Act
            var response1 = new CalcularImpostoResponse(id1, status);
            var response2 = new CalcularImpostoResponse(id2, status);

            // Assert
            response1.Should().NotBe(response2);
        }

        [Fact]
        public void ToString_DeveRetornarFormatoEsperado()
        {
            // Arrange
            var id = 42;
            var status = EPedidoTributadoStatus.Criado;
            var expectedString = $"CalcularImpostoResponse {{ Id = {id}, Status = {status} }}";

            var response = new CalcularImpostoResponse(id, status);

            // Act & Assert
            response.ToString().Should().Be(expectedString);
        }
    }
}